using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serveur.Database;
using Serveur.Model;
using System.Text.Json;

namespace Serveur.Controllers.Api
{
    [ApiController]
    [Route("api/universe")]
    public class UniverseController
    {

        [HttpPost("createuniverse")]
        public async Task<IActionResult> SignIn(
            [FromForm] string name,
            [FromForm] string password,
            [FromForm] string owner)
        {
            string response = "";

            if (await Universe.InsertUnivers(name, password, owner))
            {
                CreateUniverseSuccess data = new CreateUniverseSuccess("Universe create successfully");
                response = JsonSerializer.Serialize<CreateUniverseSuccess>(data);
            }
            else
            {
                CreateUniverseFailure data = new CreateUniverseFailure("Universe could not be created");
                response = JsonSerializer.Serialize<CreateUniverseFailure>(data);
            }

            var result = new ContentResult
            {
                Content = response,
                ContentType = "application/json; charset=UTF-8",
            };
            return result;
        }
    }
}
