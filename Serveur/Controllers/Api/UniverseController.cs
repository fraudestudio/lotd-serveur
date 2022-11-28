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
            int? maybeId = HttpContext.User.UserId();

            if (maybeId is int id)
                {
                if (await Database.Universe.InsertUniverse(universe, id))
                {
                    return new StatusCodeResult(201);
                }
                else
                {
                    return new ContentResult {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        ContentType = "text/plain",
                        Content = "can't create universe",
                    };
                }
            }
            else
            {
                return new ContentResult {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ContentType = "text/plain",
                    Content = "unknown user",
                };
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            int? maybeId = HttpContext.User.UserId();

            if (maybeId is int id)
            {
                var result = await Database.Universe.ReturnUniverse();

                return new ContentResult {
                    Content = JsonSerializer.Serialize<List<Model.Universe>>(result, Utils.Utils.DefaultJsonOptions),
                    ContentType = "application/json; charset=UTF-8",
                };
            }
            else
            {
                return new ContentResult {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ContentType = "text/plain",
                    Content = "unknown user",
                };
            }
        }

        [HttpGet("joined")]
        public async Task<IActionResult> Joined()
        {
            int? maybeId = HttpContext.User.UserId();

            if (maybeId is int id)
            {
                var result = await Database.Universe.UniversPlayer();

                return new ContentResult {
                    Content = JsonSerializer.Serialize<List<Model.Universe>>(result, Utils.Utils.DefaultJsonOptions),
                    ContentType = "application/json; charset=UTF-8",
                };
            }
            else
            {
                return new ContentResult {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ContentType = "text/plain",
                    Content = "unknown user",
                };
            }
        }

        [HttpGet("owned")]
        public async Task<IActionResult> Owned()
        {
            int? maybeId = HttpContext.User.UserId();

            if (maybeId is int id)
            {
                var result = await Database.Universe.UniversOwned();

                return new ContentResult {
                    Content = JsonSerializer.Serialize<Model.Universe>(result, Utils.Utils.DefaultJsonOptions),
                    ContentType = "application/json; charset=UTF-8",
                };
            }
            else
            {
                return new ContentResult {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ContentType = "text/plain",
                    Content = "unknown user",
                };
            }
        }
    }
}
