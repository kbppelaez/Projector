using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Projector.Controllers
{
    public class ProjectsController : Controller
    {

        [Route("/")]
        [HttpGet]
        public IActionResult Index() {
            return RedirectToAction("Projects");
        }


        [Route("projector/")]
        [Route("projector/projects")]
        [Authorize]
        public IActionResult Projects()
        {
            return View();
        }
    }
}
