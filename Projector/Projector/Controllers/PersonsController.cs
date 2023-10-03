using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projector.Core;
using Projector.Core.Persons.DTO;
using Projector.Models;

namespace Projector.Controllers
{
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

        [Route("/projector/persons/create")]
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View(new CreatePersonViewModel());
        }

        [Route("/projector/persons/create")]
        [HttpPost]
        [Authorize]
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

            if(result.IsSuccessful)
            {
                return RedirectToAction("Projects", "Projects");
            }

            return View("Create", new CreatePersonViewModel(newPerson, result.Errors));
        }
    }
}
