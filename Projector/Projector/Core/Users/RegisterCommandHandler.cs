using Projector.Core.Users.DTO;
using Projector.Data;
using System.Text.RegularExpressions;

namespace Projector.Core.Users
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IUsersService _usersService;

        public RegisterCommandHandler(ProjectorDbContext context, IUsersService usersService)
        {
            _db = context;
            _usersService = usersService;
        }
        public async Task<CommandResult> Execute(RegisterCommand newUser)
        {
            if (!string.IsNullOrEmpty(newUser.Details.UserName))
            {
                // check if email exists
                if (isExisting(newUser.Details.UserName))
                {
                    return CommandResult.Error("User is already registered.");
                }

                // check if password is matching
                if(!matching(newUser.Details.Password, newUser.Details.ConfirmPassword)) {
                    return CommandResult.Error("Passwords do not match.");
                }

                // if it matches, does it contain valid characters only
                if(isValidPassword(newUser.Details.Password))
                {
                    //VALID USER

                    //TODO:
                    //GENERATE LINK
                    //SEND LINK VIA EMAIL

                    //SAVE USER TO THE DATABASE
                    User userData = createUser(newUser.Details);
                    _db.Users.Add(userData);
                    await _db.SaveChangesAsync();

                    //CREATE PERSON ASSOCIATED
                    Person person = createPerson(newUser.Details, userData.Id);
                    _db.Persons.Add(person);
                    await _db.SaveChangesAsync();

                    return CommandResult.Success(person);
                }
            }

            return CommandResult.Error("Invalid email/password given.");
        }

        public Person createPerson(NewUserData userData, int userId)
        {
            return new Person
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                UserId = userId
            };
        }

        public User createUser(NewUserData userData)
        {
            return new User
            {
                UserName = userData.UserName,
                Password = _usersService.HashPassword(userData.Password),
            };
        }

        private bool isExisting(string userName)
        {
            return _db.Users.UserNameExists(userName);
        }

        private bool matching(string password, string confirm)
        {
            return password == confirm;
        }

        private bool isValidPassword(string password)
        {
            string pattern = @"^[!-~]{7,11}$";
            Match m = Regex.Match(password, pattern);
            return m.Success;
        }
    }
}
