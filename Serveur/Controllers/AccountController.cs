using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Model;

namespace Server.Controllers
{
    [ApiController]                 
    [Route("api/account")]          
    public class AccountController
    {
        [HttpPost("signup")]
        public IActionResult SignUp(
            [FromForm] String email,
            [FromForm] String pseudo,
            [FromForm(Name = "g-recaptcha-response")] String recaptcha_response)
        {
            Console.WriteLine($"email: {email}");
            Console.WriteLine($"pseudo: {pseudo}");
            Console.WriteLine($"captcha: {recaptcha_response}");

            return new RedirectResult("../../signup/success", false);
        }
    }
}
