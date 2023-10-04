using Microsoft.EntityFrameworkCore;
using Projector.Core.Persons.DTO;
using Projector.Data;

namespace Projector.Core.Persons
{
    public class EditPersonCommandHandler : ICommandHandler<EditPersonCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IPersonsService _personsService;

        public EditPersonCommandHandler() { }

        public EditPersonCommandHandler(ProjectorDbContext dbContext, IPersonsService personsService) {
            _db = dbContext;
            _personsService = personsService;
        }
        public async Task<CommandResult> Execute(EditPersonCommand args)
        {
            Person person = _db.Persons
                .Where(p => p.Id == args.PersonId)
                .Include(p => p.User)
                .FirstOrDefault();

            if(person == null)
            {
                return CommandResult.Error("404");
            }

            if (!isSame(person.User.UserName, args.EditedPerson.UserName))
            {
                var duplicate = _db.Users.UserNameExists(args.EditedPerson.UserName);

                if (duplicate)
                {
                    return CommandResult.Error("Email Address already exists.");
                }
                person.User.UserName = args.EditedPerson.UserName;
            }

            if (!isSame(person.FirstName, args.EditedPerson.FirstName))
            {
                person.FirstName = args.EditedPerson.FirstName;
            }

            if(!isSame(person.LastName, args.EditedPerson.LastName))
            {
                person.LastName = args.EditedPerson.LastName;
            }

            _db.Persons.Update(person);
            await _db.SaveChangesAsync();

            return CommandResult.Success(args);
        }

        private bool isSame(string s1, string s2)
        {
            return s1.Equals(s2);
        }
    }
}
