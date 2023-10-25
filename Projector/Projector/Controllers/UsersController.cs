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
        private readonly IAuthenticationService _authService;


        /* CONSTRUCTORS */
        public UsersController(ICommandBus commands, IUsersService service, ILogger<UsersController> logger, IAuthenticationService authenticationService) {
            _commands = commands;
            _usersService = service;
            _logger = logger;
            _authService = authenticationService;
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

                await _authService.SignIn(new PersonData(userPerson));

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

            var link = this.GetRouteAbsoluteUrl("Verify", "Users", new { userId = 0 });

            CommandResult result = await _commands.ExecuteAsync(
                new RegisterCommand
                {
                    Details = newUser,
                    RegisterUserBaseUrl = link.Substring(0, link.Length - 1)
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
            await _authService.SignOut();
            return RedirectToAction("SignIn", "Users");
        }

        [Route("/projector/createpassword/{userId}")]
        [HttpGet]
        public IActionResult CreatePassword(int userId, [FromQuery] string v)
        {
            if (string.IsNullOrEmpty(v))
            {
                return NotFound();
            }

            var vm = new ResetPasswordViewModel(_usersService);
            vm.Initialize(userId, v, 1);

            return vm.UserExists && vm.VerificationValid ?
                View("CreatePassword", vm) :
                NotFound();
        }

        [Route("/projector/createpassword/{userId}")]
        [HttpPost]
        public async Task<IActionResult> CreatePassword(int userId, [FromForm] PasswordData passwordData)
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
                        UserId = userId,
                        FromModule = 1  //from Create
                    });

            if (result.IsSuccessful)
            {
                return RedirectToAction("SignIn", "Users");
            }

            passwordData.Password = passwordData.ConfirmPassword = string.Empty;
            return result.Errors[0] == "INVALID" ?
                NotFound() :
                View("CreatePassword", new ResetPasswordViewModel(passwordData, result.Errors));
        }

        [Route("/projector/resetpassword/{userId}")]
        [HttpGet]
        public IActionResult ResetPassword(int userId, [FromQuery] string v)
        {
            if (string.IsNullOrEmpty(v))
            {
                return NotFound();
            }

            var vm = new ResetPasswordViewModel(_usersService);
            vm.Initialize(userId, v, 0);

            return vm.UserExists && vm.VerificationValid ?
                View("ResetPassword", vm) :
                NotFound();
        }

        [Route("/projector/resetpassword/{userId}")]
        [HttpPost]
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
                        UserId = userId,
                        FromModule = 0  //from ResetPassword
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
            return View(new VerifyUserViewModel
            {
                Token = v, UserId = userId
            });
        }

        [Route("/projector/verify/{userId}")]
        [HttpPost]
        public async Task<IActionResult> Verify(int userId, [FromForm] UserValidationData vData)
        {
            if (!ModelState.IsValid)
            {
                return View(new VerifyUserViewModel(vData, null));
            }

            CommandResult result = await _commands.ExecuteAsync(
                new VerifyUserCommand
                {
                    ValidationData = vData
                });

            return result.IsSuccessful ?
                RedirectToAction("SignIn", "Users") :
                View(new VerifyUserViewModel(vData, result.Errors));
        }

        [Route("/projector/forgotpassword")]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("/projector/forgotpassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromForm] string emailAdd)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var link = this.GetRouteAbsoluteUrl("ResetPassword", "Users", new { userId = 0 });

            await _commands.ExecuteAsync(new ForgotPasswordCommand {
                EmailAddress = emailAdd,
                ForgotPasswordBaseUrl = link.Substring(0, link.Length - 2)
            });

            return View("EmailSent", new { emailAddress = emailAdd });
        } 
    }
}
