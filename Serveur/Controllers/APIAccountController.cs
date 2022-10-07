using DB_LOTD;
using Microsoft.AspNetCore.Mvc;
using Server.Model;

namespace Serveur.Controllers
{
    [ApiController]
    [Route("API")]
    public class APIAccountController
    {
        [HttpPost("signin")]
        public IActionResult Signin([FromBody] string pseudo, [FromBody] string mdp)
        {
            IActionResult reponse = null;
            try
            {
                DBConnect db = new DBConnect();
                db.verif_USER(pseudo, mdp);
                reponse = new AcceptedResult();


            }
            catch (Exception e)
            {
                reponse = new BadRequestResult();
            }
            return reponse;
        }
    }
}
