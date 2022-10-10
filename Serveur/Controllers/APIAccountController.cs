using DB_LOTD;
using Microsoft.AspNetCore.Mvc;
using Server.Model;

namespace Serveur.Controllers
{
    [ApiController]
    [Route("account")]
    public class APIAccountController
    {
        [HttpPost("signin")]
        public IActionResult Signin([FromForm] string pseudo, [FromForm] string mdp)
        {
            DBConnect db = new DBConnect();
            
            Console.WriteLine("caca");
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
            }
            
        }
    }
}
