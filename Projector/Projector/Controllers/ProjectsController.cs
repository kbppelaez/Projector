using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projector.Core;
using Projector.Core.Projects;
using Projector.Core.Projects.DTO;
using Projector.Models;
using System.Security.Claims;

namespace Projector.Controllers
{
    public class ProjectsController : Controller
    {
        /* INTERFACE VARIABLES */
        private readonly ICommandBus _commands;
        private readonly IProjectsService _projectsService;
        private readonly ILogger _logger;

        /* CONSTRUCTORS */
        public ProjectsController(ICommandBus commands, IProjectsService projectsService, ILogger<ProjectsController> logger) { 
            _commands = commands;
            _projectsService = projectsService;
            _logger = logger;
        }


        [Route("/")]
        [HttpGet]
        public IActionResult Index() {
            return RedirectToAction("Projects");
        }


        [Route("projector/")]
        [Route("projector/projects")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Projects([FromQuery] ProjectSearchQuery args = null)
        {
            var vm = new ProjectsListViewModel(_projectsService);

            if(args != null)
            {
                args.PersonId = int.Parse(HttpContext.User.FindFirst("PersonId").Value);
            }
            await vm.Initialize(args);

            return View(vm);
        }

        [Route("/projector/projects/create")]
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateProjectViewModel());
        }

        [Route("/projector/projects/create")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProjectData project)
        {
            if (!ModelState.IsValid)
            {
                return View(new CreateProjectViewModel(project, null));
            }

            CommandResult result = await _commands.ExecuteAsync(
                new CreateProjectCommand
                {
                    Project = project,
                    PersonId = int.Parse(HttpContext.User.FindFirst("PersonId").Value)
                });

            if(result.IsSuccessful)
            {
                int newProject = (int) result.Result;
                return RedirectToAction("Assignments", "Projects", new { projectId = newProject });
            }

            return View(new CreateProjectViewModel(project, result.Errors));
        }

        [Route("/projector/projects/assignments/{projectId:int}")]
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> Assignments(int projectId)
        {
            var vm = new ProjectAssignmentsViewModel(_projectsService);
            await vm.Initialize(projectId);

            return View(projectId);
        }
    }
}
