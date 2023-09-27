using Projector.Core.Users.DTO;

namespace Projector.Models
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            Errors = Enumerable.Empty<string>();
        }

        public RegisterViewModel(NewUserData newUser, IEnumerable<string> errors)
        {
            this.NewUser = newUser;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public NewUserData NewUser { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
