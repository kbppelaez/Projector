using Microsoft.AspNetCore.Mvc;
using Projector.Core;
using Projector.Core.Persons.DTO;
using Projector.Core.Users.DTO;
using Projector.Data;
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
                Person userPerson = (Person)result.Result;
                user.Password = string.Empty;

                await _usersService.PersistLogin(new PersonData(userPerson));

                return returnUrl == null ?
                    RedirectToAction("Projects", "Projects") :
                    LocalRedirect(returnUrl);
            }

            return View(new SignInViewModel(user, result.Errors));
        }

        [Route("/projector/register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [Route("/projector/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] NewUserData newUser, [FromQuery] string returnUrl = null) {
            if (!ModelState.IsValid)
            {
                return View("Register", new RegisterViewModel(newUser, null));
            }

            CommandResult result = await _commands.ExecuteAsync(
                new RegisterCommand
                {
                    Details = newUser
                });

            if(result.IsSuccessful)
            {
                return returnUrl == null ?
                    RedirectToAction("Projects", "Projects") :
                    LocalRedirect(returnUrl);
            }

            return View(new RegisterViewModel(newUser, result.Errors));
        }

        [Route("/projector/logout")]
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _usersService.RemoveLogin();
            return RedirectToAction("SignIn", "Users");
        }

        [Route("/projector/resetpassword/{userId}")]
        [HttpGet]
        //public async Task<IActionResult> ResetPassword(int userId, [FromQuery] string v)
        public IActionResult ResetPassword(int userId, [FromQuery] string v)
        {
            if (string.IsNullOrEmpty(v))
            {
                return NotFound();
            }

            var vm = new ResetPasswordViewModel(_usersService);
            vm.Initialize(userId, v);

            return vm.UserExists && vm.VerificationValid ?
                View("ResetPassword", vm) :
                NotFound();
        }

        [Route("/projector/resetpassword/{userId}")]
        [HttpPost]
        //public async Task<IActionResult> ResetPassword(int userId, [FromQuery] string v)
        public async Task<IActionResult> ResetPassword(int userId, [FromForm] PasswordData passwordData)
        {
            if (!ModelState.IsValid)
            {
                passwordData.Password = passwordData.ConfirmPassword = string.Empty;
                return View(new ResetPasswordViewModel(passwordData, null));
            }

            CommandResult result = await _commands.ExecuteAsync(
                    new ResetPasswordCommand
                    {
                        Password = passwordData,
                        UserId = userId
                    });

            if (result.IsSuccessful)
            {
                return RedirectToAction("SignIn", "Users");
            }

            passwordData.Password = passwordData.ConfirmPassword = string.Empty;
            return result.Errors[0] == "INVALID" ?
                NotFound() :
                View("ResetPassword", new ResetPasswordViewModel(passwordData, result.Errors));
        }


        [Route("/projector/verify/{userId}")]
        [HttpGet]
        public IActionResult Verify(int userId, [FromQuery] string v)
        {
            return View(new { query = v, id = userId });
        }
    }
}
