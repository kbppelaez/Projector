using Microsoft.EntityFrameworkCore;
using Projector.Core.Persons.DTO;
using Projector.Core.Users.DTO;
using Projector.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Projector.Core.Users
{
    public class PersonsService : IPersonsService
    {
        private readonly ProjectorDbContext _db;

        public PersonsService(ProjectorDbContext db)
        {
            _db = db;
        }

        public async Task<PersonSearchResult> GetPersons(PersonSearchQuery args)
        {
            var query = _db.Persons
                .Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(args.Term))
            {
                query = query
                    .Where(p => p.FirstName.Contains(args.Term)
                        || p.LastName.Contains(args.Term));
            }

            int totalPersons = query.Count();

            query = query.OrderBy(p => p.LastName);
            query = skipPersons(query, args.Page, args.PageSize);

            return new PersonSearchResult
            {
                TotalPersons = totalPersons,
                People = await query
                                .Select(p => new PersonData {
                                    Id = p.Id,
                                    FirstName = p.FirstName,
                                    LastName = p.LastName,
                                    User = new UserData(p.User)
                                })
                                .ToArrayAsync()
            };
        }

        private IQueryable<Person> skipPersons(IQueryable<Person> query, int page, int pageSize)
        {
            return query.Skip(page * pageSize).Take(pageSize);
        }

        public async Task<PersonData> GetPerson(int personId)
        {
            Person person = await _db.Persons
                .Where(p => !p.IsDeleted
                            && p.Id == personId)
                .Include(p => p.User)
                .FirstOrDefaultAsync();

            return person == null ?
                null :
                new PersonData
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    User = new UserData(person.User)
                };
        }
    }
}
