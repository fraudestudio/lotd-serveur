using Microsoft.AspNetCore.Mvc;
using Server.Utils;

namespace Server.Controllers
{
    /// <summary>
    /// Controller for root of of the path of the API.
    /// </summary>
    public class HtmlController : Controller
    {
        /// <summary>
        /// Return the index page.
        /// </summary>
        public static String HtmlFile { get; set; } = "";

        [Route("")]
        [Route("signup")]
        [Route("signin")]
        [Route("validation")]
        [Route("myaccount")]

        /// <summary>
        /// Return the index page.
        ///</summary>
        public ActionResult Index()
        {
            using (StreamReader s = new StreamReader(HtmlController.HtmlFile))
            {
                var result = Content(s.ReadToEnd());
                result.ContentType = "text/html; charset=UTF-8";
                return result;
            }
        }
    }
}
