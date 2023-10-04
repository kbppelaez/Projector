using Projector.Core.Persons.DTO;

namespace Projector.Core
{
    public interface IPersonsService
    {
        Task<PersonData> GetPerson(int personId);
        Task<PersonSearchResult> GetPersons(PersonSearchQuery query);
        Task<PersonDetailsData> GetPersonWithProject(int personId);
    }
}
