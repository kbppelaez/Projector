using Projector.Core.Persons.DTO;
using Projector.Data;

namespace Projector.Core
{
    public interface IUsersService
    {
        bool UserExists(int userId);
        bool VerificationLinkValid(int id, string token);
        bool VerificationTokenValid(int id, string token);
        bool VerifyRegistration(int id, string link, string subcode);
        User GetUser(int userId);
    }
}
