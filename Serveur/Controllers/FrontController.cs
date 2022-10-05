using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
// using Stubble.Core.Builders;

namespace WebApplication1.Controllers
{
    public class FrontController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            var result = Content("<html><head><title>DEMO</title></head><body>HEllo world<body/></html>");
            result.ContentType = "text/html; charset=UTF-8";
            return result;
        }
    }
}
