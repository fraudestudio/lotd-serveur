using Microsoft.AspNetCore.Mvc;
using Server.Utils;
using Server.Model;
using Server.Database;
using System.Net.Mail;
using System.Text.Json;


namespace Server.Controllers
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
            else
            {
                tempPwd = await Account.CreateTemp(email, username);
            }
            
            if (!(await captcha.IsValid()))
            {
                return new RedirectResult("../../signup?error=invalidcaptcha", false);
            }

            EMail message = new EMail(
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

        [HttpGet("signin")]
        public async Task<IActionResult> SignIn()
        {
            ContentResult res = new ContentResult();
            res.Content = HttpContext.Request.Host.Host;
            res.ContentType = "text/plain";
            return res;
        }
    }
}
