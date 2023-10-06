using Projector.Core;
using Projector.Core.Users.DTO;

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

        public void Initialize(int userId, string token) {
            if(!_usersService.UserExists(userId))
            {
                UserExists = false;
                return;
            }
            UserExists = true;
            UserId = userId;

            var isNew = ! _usersService.isVerified(userId);

            if (!_usersService.VerificationValid(userId, token))
            {
                VerificationValid = false;
                return;
            }
            VerificationValid = true;
            NewPassword = new PasswordData
            {
                Token = token,
                Status = isNew ? 1 : 0,
            };
        }
    }
}
