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
            String token = Utils.Utils.RandomPassword(30);
            String response;

            if (await Account.CreateSession(UserIdentity.Get(HttpContext.User.Identity.Name).Id, token))
            {
                SignInSuccess data = new SignInSuccess(token);

                response = JsonSerializer.Serialize<SignInSuccess>(data);
            }
            else
            {
                SignInFailure data = new SignInFailure("failed to create a session");

                response = JsonSerializer.Serialize<SignInFailure>(data);
            }

            var result = new ContentResult {
                Content = response,
                ContentType = "application/json; charset=UTF-8",
            };
            return result;
        }
    }
}
