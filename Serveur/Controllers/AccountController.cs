using Microsoft.AspNetCore.Mvc;
using Server.Utils;
using System.Net.Mail;

namespace Server.Controllers
{        
    [ApiController]
    [Route("api/account")]          
    public class AccountController
    {
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(
            [FromForm] String email,
            [FromForm] String username,
            [FromForm(Name = "g-recaptcha-response")] String recaptcha_response)
        {
            // DBConnection database = new DBConnection();
            Captcha captcha = new Captcha(recaptcha_response);
            /*
            String tempPwd;
            if (database.userExistsEmail(email))
            {
                return new RedirectResult("../../signup?error=emailexists", false);
            }
            else if (database.userExists(username))
            {
                return new RedirectResult("../../signup?error=usernameexists", false);
            }
            else
            {
                tempPwd = database.createAccount(username, email);
            }
            */

            if (!(await captcha.IsValid()))
            {
                return new RedirectResult("../../signup?error=invalidcaptcha", false);
            }

            EMail message = new EMail(
                email,
                "Mot de passe provisoire",
                new PageTemplate("signup_email").render(new {
                    password = "ABCDEFGHI",
                    username = username,
                })
            );

            message.Send();

            return new RedirectResult("../../signup?success=true", false);
        }

        [HttpGet("email")]
        public IActionResult EMail()
        {
            EMail message = new EMail(
                "louis.devie@iut-dijon.u-bourgogne.fr",
                "Bonjour",
                new PageTemplate("signup_email").render(new {
                    password = "dwxfkmuoyigyfjhcvbjkliyf",
                    username = "pseudo",
                })
            );

            message.Send();

            ContentResult result = new ContentResult();
            result.Content = "ok";
            result.ContentType = "text/plain; charset=UTF-8";
            return result;
        }
    }
}
