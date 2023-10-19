using Projector.Core.Email;
using Projector.Core.Users.DTO;
using Projector.Data;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;

namespace Projector.Core.Users
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IUsersService _usersService;
        private readonly IAuthenticationService _authService;
        private readonly IEmailService _emailService;

        public RegisterCommandHandler(ProjectorDbContext context, IUsersService usersService, IAuthenticationService authenticationService, IEmailService emailService)
        {
            _db = context;
            _usersService = usersService;
            _authService = authenticationService;
            _emailService = emailService;
        }
        public async Task<CommandResult> Execute(RegisterCommand newUser)
        {
            if (!string.IsNullOrEmpty(newUser.Details.UserName))
            {
                // check if email exists
                if (isExisting(newUser.Details.UserName))
                {
                    return CommandResult.Error("User is already registered.");
                }

                // check if password is matching
                if(!matching(newUser.Details.Password, newUser.Details.ConfirmPassword)) {
                    return CommandResult.Error("Passwords do not match.");
                }

                // if it matches, does it contain valid characters only
                if(isValidPassword(newUser.Details.Password))
                {
                    //VALID USER
                    //SAVE USER TO THE DATABASE
                    User userData = createUser(newUser.Details);
                    _db.Users.Add(userData);
                    await _db.SaveChangesAsync();

                    //CREATE PERSON ASSOCIATED
                    Person person = createPerson(newUser.Details, userData.Id);
                    _db.Persons.Add(person);
                    await _db.SaveChangesAsync();

                    //GENERATE LINK
                    userData.VerificationLink = UsersHelper.GenerateVerificationLink(userData.UserName, userData.Id);

                    _db.Users.Update(userData);
                    await _db.SaveChangesAsync();

                    //TODO:
                    //SEND LINK VIA EMAIL
                    SendEmail(userData, newUser.RegisterUserBaseUrl, userData.Id);

                    return CommandResult.Success(person);
                }
            }

            return CommandResult.Error("Invalid email/password given.");
        }

        private void SendEmail(User newUser, string route, int userId)
        {
            EmailContentData emailContentData = new EmailContentData
            {
                Recipient = newUser.UserName,
                Subject = "Account Verification Link"
            };

            var link = UsersHelper.CreateLink(route + "/" + userId, HttpUtility.UrlEncode(newUser.VerificationLink.ActivationLink));

            var content = "<h3>Verify your Account</h3><br />";
            content += "<p>To start using your account, please verify your email by clicking this link: ";
            content += "<a href='" + link + "'> Verify Email </a> and entering the code below:</p>";
            content += "<h5>"+ newUser.VerificationLink.ActivationToken.Substring(0,10) +"</h5>";
            content += "<p>If you did not sign-up for our service, kindly disregard this email.</p>";

            emailContentData.Content = content;
            _emailService.SendEmail(emailContentData);
        }

        public Person createPerson(NewUserData userData, int userId)
        {
            return new Person
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                UserId = userId
            };
        }

        public User createUser(NewUserData userData)
        {
            return new User
            {
                UserName = userData.UserName,
                Password = UsersHelper.HashPassword(userData.Password),
            };
        }

        private bool isExisting(string userName)
        {
            return _db.Users.UserNameExists(userName);
        }

        private bool matching(string password, string confirm)
        {
            return password == confirm;
        }

        private bool isValidPassword(string password)
        {
            string pattern = @"^[!-~]{7,11}$";
            Match m = Regex.Match(password, pattern);
            return m.Success;
        }
    }
}
