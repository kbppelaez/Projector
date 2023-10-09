using Projector.Core.Users.DTO;
using Projector.Data;

namespace Projector.Core.Users
{
    public class VerifyUserCommandHandler : ICommandHandler<VerifyUserCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IUsersService _usersService;

        public VerifyUserCommandHandler() { }

        public VerifyUserCommandHandler(ProjectorDbContext context, IUsersService usersService) {
            _db = context;
            _usersService = usersService;
        }

        public async Task<CommandResult> Execute(VerifyUserCommand args)
        {
            if (!_usersService.UserExists(args.ValidationData.UserId))
            {
                return CommandResult.Error("INVALID");
            }

            if(!_usersService.VerifyRegistration(args.ValidationData.UserId, args.ValidationData.Token, args.ValidationData.Code))
            {
                return CommandResult.Error("INVALID");
            }

            return await VerifyUser(args.ValidationData);
        }

        public async Task<CommandResult> VerifyUser(UserValidationData cmd)
        {
            User user = _db.Users
                .Where(u => u.Id == cmd.UserId)
                .FirstOrDefault();

            user.VerificationLink = null;
            user.IsVerified = true;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return CommandResult.Success(cmd);
        }
    }
}
