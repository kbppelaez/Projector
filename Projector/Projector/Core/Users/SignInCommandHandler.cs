using Microsoft.EntityFrameworkCore;
using Projector.Core;
using Projector.Core.Users.DTO;
using Projector.Data;

namespace Projector.Core.Users
{
    public class SignInCommandHandler : ICommandHandler<SignInCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IUsersService _usersService;

        public SignInCommandHandler(ProjectorDbContext projectorDbContext, IUsersService usersService) {
            _db = projectorDbContext;
            _usersService = usersService;
        }
        public async Task<CommandResult> Execute(SignInCommand user)
        {
            if (!string.IsNullOrEmpty(user.Details.UserName))
            {
                var existingUser = await _db.Users
                    .FirstOrDefaultAsync(u => u.UserName == user.Details.UserName);

                if(existingUser == null)
                {
                    return CommandResult.Error("Invalid credentials given.");
                }

                bool passwordMatched = UsersHelper.VerifyPassword(existingUser.Password, user.Details.Password);

                if (passwordMatched)
                {
                    if (!existingUser.IsVerified)
                    {
                        return CommandResult.Error("Account is not verified. Please contact your administrator.");
                    }

                    Person existingPerson = await getPerson(existingUser.Id);
                    return CommandResult.Success(existingPerson);
                }

                return CommandResult.Error("Invalid credentials given.");
            }

            return CommandResult.Error("Invalid credentials given.");            
        }

        private async Task<Person> getPerson(int id)
        {
            return await _db.Persons
                .Where(p => p.UserId == id)
                .FirstOrDefaultAsync();
        }
    }
}
