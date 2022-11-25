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
                    Template.Get("signup_email.html").Render(username, tempPwd)
                );

                message.Send();

                return new RedirectResult("../../signup?success=true", false);
            }
        }

        [Authorize(AuthenticationSchemes="Basic")]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn()
        {
            String token = Utils.Utils.RandomPassword(30);
            ContentResult result = new ContentResult
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ContentType = "text/plain",
                Content = "No result",
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
                            result.Content = JsonSerializer.Serialize<SignInSuccess>(
                                new SignInSuccess
                                {
                                    Validated = true,
                                    SessionToken = token,
                                },
                                Utils.Utils.DefaultJsonOptions
                            );
                            result.ContentType = "application/json; charset=UTF-8";
                            result.StatusCode = StatusCodes.Status200OK;
                        }
                        else
                        {
                            result.Content = "Couldn't create session";
                            result.StatusCode = StatusCodes.Status503ServiceUnavailable;
                        }
                    }
                    else
                    {
                        result.Content = JsonSerializer.Serialize<SignInSuccess>(
                            new SignInSuccess
                            {
                                Validated = true,
                                SessionToken = token,
                            },
                            Utils.Utils.DefaultJsonOptions
                        );
                        result.ContentType = "application/json; charset=UTF-8";
                        result.StatusCode = StatusCodes.Status200OK;
                    }

                }
            }

            return result;
        }
    }
}
