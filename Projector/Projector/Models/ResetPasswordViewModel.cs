using Projector.Core;
using Projector.Core.Users.DTO;
using Projector.Data;

namespace Projector.Models
{
    public class ResetPasswordViewModel
    {
        private readonly IUsersService _usersService;

        public ResetPasswordViewModel() {
            Errors = Enumerable.Empty<string>();
        }

        public ResetPasswordViewModel(IUsersService usersService) {
            _usersService = usersService;
            Errors = Enumerable.Empty<string>();
        }

        public ResetPasswordViewModel(PasswordData newPassword, IEnumerable<string> errors) {
            NewPassword = newPassword;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public PasswordData NewPassword {  get; set; }
        public int UserId { get; set; }

        public IEnumerable<string> Errors { get; set; }
        public bool UserExists { get; set; }
        public bool VerificationValid {  get; set; }

        private const int CREATEPASSWORD = 1;

        public void Initialize(int userId, string token, int from) {
            User existingUser = _usersService.GetUser(userId);

            if (existingUser == null)
            {
                UserExists = false;
                return;
            }
            UserExists = true;
            UserId = userId;

            var isNew = !existingUser.IsVerified;
            if(isNew && from != CREATEPASSWORD) {
                VerificationValid = false;
                return;
            }

            if (!_usersService.VerificationLinkValid(userId, token))
            {
                VerificationValid = false;
                return;
            }

            VerificationValid = true;
            NewPassword = new PasswordData
            {
                Token = existingUser.VerificationLink.ActivationToken,
                Status = from,
            };
        }
    }
}