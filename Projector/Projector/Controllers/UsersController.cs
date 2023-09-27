using Microsoft.AspNetCore.Mvc;
using Projector.Core;
using Projector.Core.Users.DTO;
using Projector.Models;

namespace Projector.Controllers
{
    public class UsersController : Controller
    {
        /* Interfaces */
        private readonly ICommandBus _commands;
        private readonly IUsersService _usersService;
        private readonly ILogger<UsersController> _logger;

        
        /* CONSTRUCTORS */
        public UsersController(ICommandBus commands, IUsersService service, ILogger<UsersController> logger) {
            _commands = commands;
            _usersService = service;
            _logger = logger;
        }

        /* METHODS */
        [Route("/projector/signin")]
        [HttpGet]
        public IActionResult SignIn()
        {

            return View(new SignInViewModel());
        }

        [Route("/projector/signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] UserData user, [FromQuery] string returnUrl = null)
        {
            if(!ModelState.IsValid)
            {
                return View("SignIn", new SignInViewModel(user, null));
            }

            CommandResult result = await _commands.ExecuteAsync(
                new SignInCommand
                {
                    Details = user
                });

            if (result.IsSuccessful)
            {
                // TODO:
                // Get Person Associated with the User
                await _usersService.PersistLogin(user);
                user.Password = string.Empty;

                if(returnUrl == null)
                {
                    return Redirect("/projector/projects");
                }

                return LocalRedirect(returnUrl);
            }

            return View(new SignInViewModel(user, result.Errors));
        }
    }
}
