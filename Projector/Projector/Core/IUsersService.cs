using Projector.Core.Persons.DTO;

namespace Projector.Core
{
    public interface IUsersService
    {
        bool VerifyPassword(string hashed, string input);
        string HashPassword(string password);
        Task PersistLogin(PersonData user);
        Task RemoveLogin();
        string GenerateActivationToken(string email, DateTime now);
        bool UserExists(int userId);
        bool VerificationValid(int id, string token);
        bool isVerified(int userId);
    }
}
