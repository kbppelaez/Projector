using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projector.Core;
using Projector.Core.Persons;
using Projector.Core.Projects.DTO;
using Projector.Models;
using System.Security.Claims;

namespace Projector.Controllers
{
    [Authorize]
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
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateProjectViewModel());
        }

        [Route("/projector/projects/create")]
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
        public async Task<IActionResult> Assignments(int projectId)
        {
            var vm = new ProjectAssignmentsViewModel(_projectsService);
            await vm.Initialize(projectId);

            if(!vm.ProjectExists)
            {
                return NotFound();
            }

            return View(vm);
        }

        /*

        [Route("/projector/projects/assignments/{projectId:int}/add")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddAssignedPerson([FromBody]AddAssignedPersonCommand args)
        {
            // in the frontend, make sure that proper headers are set
            //   -  Content-Type: application/json
            //   -  Accepts: [ application/json | text/html ]

            CommandResult result = await _commands.ExecuteAsync(args);

            // return JSON response?
            // return Json(result); // maybe a viewmodel??

            // perhaps use HTMX?
            return PartialView("_ProjectAssignments", result);
        }

        public class AddAssignedPersonCommand
        {
            public int ProjectId { get; set; }
            public int PersonId { get; set; }
        }

        public class ProjectAssignmentStateData
        {
            public PersonData[] Assigned { get; set; }
            public PersonData[] Unassigned { get; set; }
        }
        */
    }
}
