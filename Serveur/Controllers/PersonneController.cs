using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [ApiController]                 //Indique que votre classe est un controller
    [Route("Personne")]             //Adresse de votre controller, dans notre cas, pour appeler une méthode de ce controller, il faudra donc appeler : https://localhost:7034/Personne/RouteDeLaMethode
    public class PersonneController
    {
        /// <summary>
        /// Cette méthode permet au client de demander à l'API une personne en lui fournissant un id 
        /// (définie ici en dur dans cet exemple, mais l'API pourrait par exemple questionner une BDD)
        /// </summary>
        /// <returns>La réponse de l'API</returns>
        [HttpGet("DonneMoiLaPersonne")]
        public IActionResult Donne(int id)
        {
            //Création de la personne en dure (remplaçable par un appel BDD par exemple)
            Personne personneDemandee = null;
            switch(id)
            {
                case 0: personneDemandee = new Personne("Alice", "Martin"); break;
                case 1: personneDemandee = new Personne("Bob", "Mazone"); break;
            }

            //Création de la réponse
            IActionResult actionResult = new NoContentResult();     //Réponse par défaut dans notre cas (pas de content car la personne n'existe pas)
            if (personneDemandee != null)
            {
                actionResult = new JsonResult(personneDemandee);    //Si la personne existe, l'API la renvoie en JSon
            }
            return actionResult;
        }

        /// <summary>
        /// Cette méthode permet au client d'envoyer une personne pour (par exemple) que l'API l'ajoute à la BDD
        /// </summary>
        /// <param name="jsonString">le JSon de la personne</param>
        /// <returns>La réponse de l'API</returns>
        [HttpPost("VoiciUnePersonne")]
        public IActionResult VoiciUnePersonne([FromBody] string jsonString)
        {
            IActionResult reponse = null;
            try
            {
                Personne personne = JsonConvert.DeserializeObject<Personne>(jsonString);
                reponse = new AcceptedResult();
                //La on pourrait mettre le code pour ajouter la personne à la BDD
            }
            catch(Exception e)
            {
                reponse = new BadRequestResult();
            }
            return reponse;
        }
    }
}
