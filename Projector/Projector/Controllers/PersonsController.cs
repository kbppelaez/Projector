using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projector.Core;
using Projector.Core.Persons.DTO;
using Projector.Models;

namespace Projector.Controllers
{
    [Authorize]
    public class PersonsController : Controller
    {
        private readonly ICommandBus _commands;
        private readonly IPersonsService _personsService;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(ICommandBus commandbus, IPersonsService personsService, ILogger<PersonsController> logger) {
            _commands = commandbus;
            _personsService = personsService;
            _logger = logger;
        }

        [Route("/projector/persons/")]
        [HttpGet]
        public async Task<IActionResult> Persons([FromQuery] PersonSearchQuery args)
        {
            var personId = int.Parse(HttpContext.User.FindFirst("PersonId").Value);
            var vm = new PersonsListViewModel(_personsService);
            await vm.Initialize(args, personId);

            return View("Persons", vm);
        }

        [Route("/projector/persons/create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePersonViewModel());
        }

        [Route("/projector/persons/create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] NewPersonData newPerson)
        {
            if(!ModelState.IsValid)
            {
                return View("Create", new CreatePersonViewModel(newPerson, null));
            }

            var link = this.GetRouteAbsoluteUrl("CreatePassword", "Users", new { userId = 0 });

            CommandResult result = await _commands.ExecuteAsync(
                new CreatePersonCommand
                {
                    NewPerson = newPerson,
                    CreateNewPasswordBaseUrl = link.Substring(0, link.Length - 1)
                });

            return result.IsSuccessful ?
                RedirectToAction("Persons", "Persons") :
                View("Create", new CreatePersonViewModel(newPerson, result.Errors));
        }

        [Route("/projector/persons/{personId:int}/edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int personId)
        {
            var vm = new EditPersonViewModel(_personsService);
            await vm.Initialize(personId);

            return vm.PersonExists ?
                View("Edit", vm) :
                NotFound();
        }

        [Route("/projector/persons/{personId:int}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] NewPersonData person, int personId)
        {
            var currPersonId = int.Parse(HttpContext.User.FindFirst("PersonId").Value);
            if(currPersonId == personId)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return View("Edit", new EditPersonViewModel(person, null));
            }

            CommandResult result = await _commands.ExecuteAsync(
                    new EditPersonCommand
                    {
                        EditedPerson = person,
                        PersonId = personId
                    }
                );

            return result.IsSuccessful ?
                RedirectToAction("Details", "Persons", new {personId = personId}) :
                View("Edit", new EditPersonViewModel(person, result.Errors) );
        }

        [Route("/projector/persons/{personId:int}/delete")]
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int personId)
        {
            var currPersonId = int.Parse(HttpContext.User.FindFirst("PersonId").Value);
            if (currPersonId == personId)
            {
                return NotFound();
            }
            var vm = new DeletePersonViewModel(_personsService);
            await vm.Initialize(personId);

            return vm.PersonExists ?
                View("Delete", vm) :
                NotFound();
        }

        [Route("/projector/persons/{personId:int}/delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int personId)
        {
            var currPersonId = int.Parse(HttpContext.User.FindFirst("PersonId").Value);
            if (currPersonId == personId)
            {
                return NotFound();
            }

            CommandResult result = await _commands.ExecuteAsync(
                    new DeletePersonCommand
                    {
                        Person = await _personsService.GetPerson(personId)
                    }
                );

            return result.IsSuccessful ?
                RedirectToAction("Persons", "Persons") :
                NotFound(result.Errors);
        }

        [Route("/projector/persons/{personId:int}")]
        [HttpGet]
        public async Task<IActionResult> Details(int personId)
        {
            var userPersonId = int.Parse(HttpContext.User.FindFirst("PersonId").Value);
            var vm = new PersonDetailsViewModel(_personsService);
            await vm.Initialize(personId, userPersonId);

            return vm.PersonExists ?
                View("Details", vm) :
                NotFound();
        }
    }
}
