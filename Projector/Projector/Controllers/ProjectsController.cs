using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Projector.Core;
using Projector.Core.Persons;
using Projector.Core.Persons.DTO;
using Projector.Core.Projects.DTO;
using Projector.Data;
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
                return RedirectToAction("ProjectDetails", "Projects", new { projectId = newProject });
            }

            return View(new CreateProjectViewModel(project, result.Errors));
        }

        [Route("/projector/projects/{projectId:int}/edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int projectId)
        {
            var personId = int.Parse(HttpContext.User.FindFirst("PersonId").Value);
            var vm = new EditProjectViewModel(_projectsService);
            await vm.Initialize(projectId, personId);
            
            return vm.ProjectExists ?
                View("Edit", vm) :
                NotFound();
        }

        [Route("/projector/projects/{projectId:int}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] ProjectData project)
        {
            if (!ModelState.IsValid)
            {
                return View(new EditProjectViewModel(project, null));
            }

            var personId = int.Parse(HttpContext.User.FindFirst("PersonId").Value);
            CommandResult result = await _commands.ExecuteAsync(
                    new EditProjectCommand
                    {
                        PersonId = personId,
                        Project = project
                    }
                );

            if(result.IsSuccessful)
            {
                return RedirectToAction("ProjectDetails", "Projects", new { projectId = project.Id });
            }

            return result.Errors[0] == "404" ?
                NotFound() :
                View(new EditProjectViewModel(project, result.Errors));
        }

        [Route("/projector/projects/{projectId:int}/delete")]
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int projectId)
        {
            var personId = int.Parse(HttpContext.User.FindFirst("PersonId").Value);
            var vm = new DeleteProjectViewModel(_projectsService);
            await vm.Initialize(projectId, personId);

            return vm.ProjectExists ?
                View("Delete", vm):
                NotFound();
        }

        [Route("/projector/projects/{projectId:int}/delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int projectId)
        {
            var personId = int.Parse(HttpContext.User.FindFirst("PersonId").Value);

            CommandResult result = await _commands.ExecuteAsync(
                    new DeleteProjectCommand
                    {
                        Project = await _projectsService.GetProjectDetails(projectId, personId)
                    }
                );

            return result.IsSuccessful ?
                RedirectToAction("Projects", "Projects") :
                NotFound(result.Errors);
        }

        [Route("/projector/projects/assignments/{projectId:int}")]
        [HttpGet]
        public async Task<IActionResult> ProjectDetails(int projectId)
        {
            //TODO check if current user's ID is included in assignees
            var vm = new ProjectDetailsViewModel(_projectsService);
            await vm.Initialize(projectId);

            if(!vm.ProjectExists)
            {
                return NotFound();
            }

            return View(vm);
        }

        [Route("/projector/projects/assignments/add")]
        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] AssigneeData change)
        {
            CommandResult result = await _commands.ExecuteAsync(
                new AssignPersonCommand(change));

            if(!result.IsSuccessful)
            {
                return BadRequest(result.Errors[0]);
            }

            var vm = new ProjectDetailsViewModel(_projectsService);
            await vm.Initialize(change.ProjectId);

            return PartialView("_Assignments", vm);
        }

        [Route("/projector/projects/assignments/remove")]
        [HttpPost]
        public async Task<IActionResult> Remove([FromBody] AssigneeData change)
        {
            CommandResult result = await _commands.ExecuteAsync(
                new RemovePersonCommand(change));

            if (!result.IsSuccessful)
            {
                return BadRequest(result.Errors[0]);
            }

            var currUser = int.Parse(HttpContext.User.FindFirst("PersonId").Value);
            if(currUser == change.PersonId)
            {
                return Content(Url.Action("Projects", "Projects"), "text/html");
            }

            var vm = new ProjectDetailsViewModel(_projectsService);
            await vm.Initialize(change.ProjectId);

            return PartialView("_Assignments", vm);
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
