using Microsoft.AspNetCore.Mvc;
using Server.Utils;
using Server.Model;
using Server.Database;
using System.Net.Mail;
using System.Text.Json;
using System.Text;

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
            else if (!(await captcha.IsValid()))
            {
                return new RedirectResult("../../signup?error=invalidcaptcha", false);
            }
            else
            {
                tempPwd = await Account.CreateTemp(email, username);

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
        }

        [HttpPost("signin")]
        public IActionResult SignIn()
        {
            Console.WriteLine("debug");
            foreach (String str in Request.Headers.Authorization)
            {
                Console.WriteLine(str);
            }
            /*Console.WriteLine(
                Encoding.UTF8.GetString(
                    Convert.FromBase64String(
                        Request.Headers.Authorization
                    )
                )
            );*/

            /*DBConnect db = new DBConnect();
            
            try
            {
                Console.WriteLine(pseudo + " " + mdp);
                if (db.VerifJoueurConnexion(pseudo, mdp))
                {
                    Console.WriteLine("okok");
                    return new AcceptedResult();
                    
                }
                else
                {
                    Console.WriteLine("Nop");
                    return new BadRequestResult();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new BadRequestResult();
            }*/
            
            return new AcceptedResult();
        }
    }
}
