using Microsoft.EntityFrameworkCore;
using Projector.Core.Users.DTO;
using Projector.Data;

namespace Projector.Core.Users
{
    public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IUsersService _usersService;

        public ForgotPasswordCommandHandler() { }

        public ForgotPasswordCommandHandler(ProjectorDbContext context, IUsersService usersService) {
            _db = context;
            _usersService = usersService;
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
                .FirstOrDefault();

            if(existingUser == null || existingUser.Person.IsDeleted)
            {
                return CommandResult.Error("User does not exist.");
            }

            existingUser.VerificationLink = _usersService
                .GenerateVerificationLink(email, existingUser.Id);

            _db.Users.Update(existingUser);
            await _db.SaveChangesAsync();

            //TODO
            //SEND EMAIL HERE

            return CommandResult.Success(email);
        }
    }
}
