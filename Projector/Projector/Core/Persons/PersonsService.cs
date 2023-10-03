using Projector.Data;

namespace Projector.Core.Users
{
    public class PersonsService : IPersonsService
    {
        private readonly ProjectorDbContext _db;

        public PersonsService(ProjectorDbContext db)
        {
            _db = db;
        }
    }
}
