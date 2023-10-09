using Microsoft.EntityFrameworkCore;
using Projector.Core.Email;
using Projector.Core.Users.DTO;
using Projector.Data;

namespace Projector.Core.Users
{
    public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IUsersService _usersService;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler() { }

        public ForgotPasswordCommandHandler(ProjectorDbContext context, IUsersService usersService, IEmailService emailService) {
            _db = context;
            _usersService = usersService;
            _emailService = emailService;
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
                existingUser.VerificationLink = _usersService
                    .GenerateVerificationLink(email, existingUser.Id, 1);
            }
            else
            {
                var temporaryLink = _usersService
                .GenerateVerificationLink(email, existingUser.Id, 1);

                existingUser.VerificationLink.ActivationToken = temporaryLink.ActivationToken;
                existingUser.VerificationLink.ActivationLink = temporaryLink.ActivationLink;
                existingUser.VerificationLink.ExpiryDate = temporaryLink.ExpiryDate;
            }

            _db.Users.Update(existingUser);
            await _db.SaveChangesAsync();

            //TODO
            //SEND EMAIL HERE
            SendEmail(existingUser);

            return CommandResult.Success(email);
        }

        private void SendEmail(User existingUser)
        {
            EmailContentData emailContentData = new EmailContentData
            {
                Recipient = existingUser.UserName,
                Subject = "Reset Password Link"
            };

            var content = "<h3>Reset Password Link</h3><br/>";
            content += "<p>To reset your password, please clink this link: ";
            content += "<a href='" + existingUser.VerificationLink.ActivationLink + "'>Reset Password</a>.</p>";
            content += "<p>If you did not request this reset password link, kindly disregard this email.</p>";

            emailContentData.Content = content;

            _emailService.SendEmail(emailContentData);
        }
    }
}
