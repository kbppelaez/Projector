using Projector.Core.Users.DTO;

namespace Projector.Models
{
    public class SignInViewModel
    {
        /* SERVICES */

        /* CONSTRUCTORS */
        public SignInViewModel() {
            Errors = Enumerable.Empty<string>();
        }

        public SignInViewModel(UserData user, IEnumerable<string> errors)
        {
            User = user;
            Errors = errors ?? Enumerable.Empty<string>();
        }



        /* PROPERTIES */
        public UserData User { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
