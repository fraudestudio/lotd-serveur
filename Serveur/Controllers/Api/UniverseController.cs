using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Auth;
using Serveur.Database;
using Serveur.Model;
using System.Text.Json;

namespace Serveur.Controllers.Api
{
    [ApiController]
    [Route("api/universe")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UniverseController : Controller
    {

        [HttpPost("create")]
        public async Task<IActionResult> Create(Model.Universe universe)
        {
            string response = "";
            
            int id = HttpContext.User.Id();

            if (await Database.Universe.InsertUniverse(name, password, id))
            {
                return new StatusCodeResult(201);
            }
            else
            {
                return new ContentResult {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ContentType = "text/plain",
                    Content = "CAN'T CREATE UNIVERSE",
                };
            }
        }
    }
}
