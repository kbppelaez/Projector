using Microsoft.EntityFrameworkCore;
using Projector.Core.Persons.DTO;
using Projector.Data;

namespace Projector.Core.Persons
{
    public class DeletePersonCommandHandler : ICommandHandler<DeletePersonCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IPersonsService _personsService;

        public DeletePersonCommandHandler(ProjectorDbContext context, IPersonsService personsService)
        {
            _db = context;
            _personsService = personsService;
        }

        public async Task<CommandResult> Execute(DeletePersonCommand toDelete)
        {
            if(toDelete == null)
            {
                return CommandResult.Error("404");
            }

            Person person = await _db.Persons
                .Where(p => p.Id == toDelete.Person.Id)
                .Include(p => p.User)
                .FirstOrDefaultAsync();

            person.IsDeleted = true;
            person.User.IsVerified = false;

            _db.Persons.Update(person);
            _db.Users.Update(person.User);

            await _db.SaveChangesAsync();

            return CommandResult.Success(toDelete);
        }
    }
}
