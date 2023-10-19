using Projector.Core.Email;
using Projector.Core.Persons.DTO;
using Projector.Core.Users;
using Projector.Data;
using System.Web;

namespace Projector.Core.Persons
{
    public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IEmailService _emailService;
        private readonly IAuthenticationService _authService;

        public CreatePersonCommandHandler(ProjectorDbContext context, IEmailService emailService, IAuthenticationService authenticationService)
        {
            _db = context;
            _emailService = emailService;
            _authService = authenticationService;
        }
        public async Task<CommandResult> Execute(CreatePersonCommand args)
        {
            var duplicateEmail = _db.Users.UserNameExists(args.NewPerson.UserName);
            if(duplicateEmail)
            {
                return CommandResult.Error("Email Address already exists.");
            }

            User newUser = new User
            {
                UserName = args.NewPerson.UserName,
                Password = UsersHelper.HashPassword(args.NewPerson.UserName+DateTime.Now.ToString()),
                IsVerified = false
            };

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            Person newPerson = new Person
            {
                FirstName = args.NewPerson.FirstName,
                LastName = args.NewPerson.LastName,
                UserId = newUser.Id,
                IsDeleted = false
            };

            _db.Persons.Add(newPerson);
            await _db.SaveChangesAsync();

            //GENERATE VERIFICATION LINK
            newUser.VerificationLink = UsersHelper
                .GenerateVerificationLink(newUser.UserName, newUser.Id);

            _db.Users.Update(newUser);
            await _db.SaveChangesAsync();

            //TODO:
            //SEND EMAIL
            SendEmail(newUser, args.CreateNewPasswordBaseUrl, newUser.Id);

            return CommandResult.Success(newPerson);
        }

        private void SendEmail(User newUser, string route, int userId)
        {
            EmailContentData emailContentData = new EmailContentData
            {
                Recipient = newUser.UserName,
                Subject = "Account Verification and Password Creation Link"
            };

            var link = UsersHelper.CreateLink(route + "/" + userId, HttpUtility.UrlEncode(newUser.VerificationLink.ActivationLink));

            var content = "<h3>Verify your Account</h3><br/>";
            content += "<p>To start using your account, please verify your email and create a password by clicking this link: ";
            content += "<a href='" + link + "'>Create Password</a>.</p>";
            content += "<p>If you did not request this account verification, kindly disregard this email.</p>";

            emailContentData.Content = content;

            _emailService.SendEmail(emailContentData);
        }
    }
}
