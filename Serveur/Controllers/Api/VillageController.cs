using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Auth;
using Server.Database;
using Server.Model;
using Server.Utils;
using System.Text.Json;

namespace Server.Controllers.Api
{
    [ApiController]
    [Route("api/village")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class VillageController : Controller
    {

        /// <summary>
        /// Send a request to create a village
        /// </summary>
        /// <param name="village">Village paramater</param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateVillage(Model.Village village)
        {
            int? maybeId = HttpContext.User.UserId();

            if (maybeId is int id)
            {
                if (await Database.VillageDB.InsertVillage(village, id))
                {
                    return new StatusCodeResult(201);
                }
                else
                {
                    return new ContentResult
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        ContentType = "text/plain",
                        Content = "can't create universe",
                    };
                }
            }
            else
            {
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ContentType = "text/plain",
                    Content = "unknown user",
                };
            }
        }

        /// <summary>
        /// Send the village name
        /// </summary>
        /// <param name="idVill">id of the village</param>
        /// <returns></returns>
        [HttpGet("name/{idVill}")]
        public async Task<IActionResult> GetVillageName(int idVill)
        {
            int? maybeId = HttpContext.User.UserId();

            if (maybeId is int id)
            {

                var result = new { Name = await Database.Universe.PlayerVillageName(id, idVill) };

                return new ContentResult
                {
                    Content = JsonSerializer.Serialize(result),
                    ContentType = "application/json; charset=UTF-8",
                };
            }
            else
            {
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ContentType = "text/plain",
                    Content = "unknown user",
                };
            }
        }
    }
}
