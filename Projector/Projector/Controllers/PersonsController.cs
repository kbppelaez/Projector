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

            CommandResult result = await _commands.ExecuteAsync(
                new CreatePersonCommand
                {
                    NewPerson = newPerson
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
            if(!ModelState.IsValid)
            {
                return View("Edit", new EditPersonViewModel(person, null));
            }
            var vm = new EditPersonViewModel(_personsService);
            await vm.Initialize(personId);

            return vm.PersonExists ?
                View("Edit", vm) :
                NotFound();
        }
    }
}
