using Projector.Core.Persons.DTO;

namespace Projector.Core
{
    public interface IAuthenticationService
    {
        Task SignIn(PersonData user);
        Task SignOut();
    }
}
