using Projector.Core.Users.DTO;
using Projector.Data;
using Projector.Migrations;

namespace Projector.Core.Users
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IUsersService _usersService;

        public ResetPasswordCommandHandler() { }

        public ResetPasswordCommandHandler(ProjectorDbContext context, IUsersService usersService) {
            _db = context;
            _usersService = usersService;
        }
        public async Task<CommandResult> Execute(ResetPasswordCommand args)
        {
            //password matches
            if (args.Password.Password != args.Password.ConfirmPassword)
            {
                return CommandResult.Error("Password does not match.");
            }

            //user exists
            if (!_usersService.UserExists(args.UserId))
            {
                return CommandResult.Error("INVALID");
            }
            
            //verification valid?
            if(!_usersService.VerificationValid(args.UserId, args.Password.Token))
            {
                return CommandResult.Error("INVALID");
            }

            return await changePassword(args);
        }

        private async Task<CommandResult> changePassword(ResetPasswordCommand args)
        {
            User user = _db.Users
                .Where(u => u.Id == args.UserId)
                .FirstOrDefault();

            user.Password = _usersService.HashPassword(args.Password.Password);

            if (isNewAccount(args.Password.Status))
            {
                user.IsVerified = true;
            }
            user.VerificationLink = null;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return CommandResult.Success(args);
        }

        private bool isNewAccount(int  status)
        {
            return status == 1;
        }
    }
}
