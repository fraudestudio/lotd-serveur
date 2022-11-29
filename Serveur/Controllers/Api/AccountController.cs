using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Server.Utils;
using Server.Model;
using Server.Auth;
using Server.Database;
using System.Net.Mail;
using System.Text.Json;
using System.Text;
using System.Security.Principal;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Server.Controllers.Api
{
    [ApiController]
    [Route("api/account")]          
    public class AccountController : Controller
    {
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(
            [FromForm] String email,
            [FromForm] String username,
            [FromForm(Name = "g-recaptcha-response")] String recaptcha_response)
        {
            Captcha captcha = new Captcha(recaptcha_response);
            
            String tempPwd;
            if (await Account.UserExistsEmail(email))
            {
                return new RedirectResult("../../signup?error=emailexists", false);
            }
            else if (await Account.UserExists(username))
            {
                return new RedirectResult("../../signup?error=usernameexists", false);
            }
            else if (!(await captcha.IsValid()))
            {
                return new RedirectResult("../../signup?error=invalidcaptcha", false);
            }
            else
            {
                tempPwd = await Account.CreateTemp(email, username);

                Email message = new Email(
                    email,
                    "Mot de passe provisoire",
                    new PageTemplate("signup_email").render(new {
                        password = tempPwd,
                        username = username,
                    })
                );

                message.Send();

                return new RedirectResult("../../signup?success=true", false);
            }
        }

        [Authorize(AuthenticationSchemes="Basic")]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn()
        {
            String token = Util.RandomPassword(30);
            SignIn result = new SignIn
            {
                Success = false,
                Reason = "Invalid ticket",
            }; 

            int? maybeId = HttpContext.User.UserId();
            bool? maybeValidated = HttpContext.User.IsValidated();

            if (maybeId is int id)
            {
                if (maybeValidated is bool validated)
                {
                    if (validated)
                    {
                        if (await Account.CreateSession(id, token))
                        {
                            result = new SignIn
                            {
                                Success = true,
                                Validated = true,
                                SessionToken = token,
                            };
                        }
                        else
                        {
                            result = new SignIn
                            {
                                Success = false,
                                Reason = "Couldn't create session",
                            };
                        }
                    }
                    else
                    {
                        result = new SignIn
                        {
                            Success = true,
                            Validated = false,
                        };
                    }

                }
            }

            var contentResult = new ContentResult {
                Content = JsonSerializer.Serialize<SignIn>(result, Util.DefaultJsonOptions),
                ContentType = "application/json; charset=UTF-8",
            };
            return contentResult;
        }
    }
}
