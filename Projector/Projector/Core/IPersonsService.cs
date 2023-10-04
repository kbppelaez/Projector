using Projector.Core.Persons.DTO;

namespace Projector.Core
{
    public interface IPersonsService
    {
        Task<PersonSearchResult> GetPersons(PersonSearchQuery query);
    }
}
