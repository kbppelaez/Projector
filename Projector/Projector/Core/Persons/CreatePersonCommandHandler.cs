using Projector.Core.Persons.DTO;
using Projector.Data;

namespace Projector.Core.Persons
{
    public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IPersonsService _personsService;

        public CreatePersonCommandHandler(ProjectorDbContext context, IPersonsService personsService)
        {
            _db = context;
            _personsService = personsService;
        }
        public async Task<CommandResult> Execute(CreatePersonCommand args)
        {
            var duplicateEmail = _db.Users.UserNameExists(args.NewPerson.UserName);
            if(duplicateEmail)
            {
                return CommandResult.Error("Email Address already exists.");
            }

            User newUser = new User
            {
                UserName = args.NewPerson.UserName,
                Password = "temp",
            };

            // TODO
            // Add isverified column
            // generate temporary password

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            Person newPerson = new Person
            {
                FirstName = args.NewPerson.FirstName,
                LastName = args.NewPerson.LastName,
                UserId = newUser.Id
            };

            _db.Persons.Add(newPerson);
            await _db.SaveChangesAsync();

            return CommandResult.Success(newPerson);
        }
    }
}
