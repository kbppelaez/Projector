using Microsoft.EntityFrameworkCore;
using Projector.Core.Email;
using Projector.Core.Users.DTO;
using Projector.Data;
using System.Web;

namespace Projector.Core.Users
{
    public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IUsersService _usersService;
        private readonly IEmailService _emailService;
        private readonly IAuthenticationService _authService;

        public ForgotPasswordCommandHandler() { }

        public ForgotPasswordCommandHandler(ProjectorDbContext context, IUsersService usersService, IEmailService emailService, IAuthenticationService authenticationService) {
            _db = context;
            _usersService = usersService;
            _emailService = emailService;
            _authService = authenticationService;
        }

        public async Task<CommandResult> Execute(ForgotPasswordCommand args)
        {
            var email = args.EmailAddress;
            if(string.IsNullOrEmpty(email))
            {
                return CommandResult.Error("Invalid email given.");
            }
            
            User existingUser = _db.Users
                .Where(u => u.UserName == email)
                .Include(u => u.Person)
                .Include(u => u.VerificationLink)
                .FirstOrDefault();

            if(existingUser == null || existingUser.Person.IsDeleted)
            {
                return CommandResult.Error("User does not exist.");
            }

            if(existingUser.VerificationLink == null)
            {
                existingUser.VerificationLink = UsersHelper
                    .GenerateVerificationLink(email, existingUser.Id);
            }
            else
            {
                var temporaryLink = UsersHelper
                .GenerateVerificationLink(email, existingUser.Id);

                existingUser.VerificationLink.ActivationToken = temporaryLink.ActivationToken;
                existingUser.VerificationLink.ActivationLink = temporaryLink.ActivationLink;
                existingUser.VerificationLink.ExpiryDate = temporaryLink.ExpiryDate;
            }

            _db.Users.Update(existingUser);
            await _db.SaveChangesAsync();

            //TODO
            //SEND EMAIL HERE
            SendEmail(existingUser, _authService.GetRoute("ResetPassword","Users", new {userId = existingUser.Id}));

            return CommandResult.Success(email);
        }

        private void SendEmail(User existingUser, string route)
        {
            EmailContentData emailContentData = new EmailContentData
            {
                Recipient = existingUser.UserName,
                Subject = "Reset Password Link"
            };

            var link = UsersHelper.CreateLink(route, HttpUtility.UrlEncode(existingUser.VerificationLink.ActivationLink));

            var content = "<h3>Reset Password Link</h3><br/>";
            content += "<p>To reset your password, please clink this link: ";
            content += "<a href='" + link + "'>Reset Password</a>.</p>";
            content += "<p>If you did not request this reset password link, kindly disregard this email.</p>";

            emailContentData.Content = content;

            _emailService.SendEmail(emailContentData);
        }
    }
}
