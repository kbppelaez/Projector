using Projector.Core;
using Projector.Core.Persons.DTO;
using Projector.Core.Projects.DTO;

namespace Projector.Models
{
    public class PersonDetailsViewModel
    {
        private readonly IPersonsService _personsService;

        public PersonDetailsViewModel() { }

        public PersonDetailsViewModel(IPersonsService personsService) {
            _personsService = personsService;
        }

        /* PROPERTIES */
        public PersonDetailsData Details { get; set; }
        public bool PersonExists { get; set; }
        public int UserPersonId { get; set; }

        /* METHODS */
        public async Task Initialize(int personId, int currId)
        {
            UserPersonId = currId;
            Details = await _personsService.GetPersonWithProject(personId);
            PersonExists = Details != null;
        }
    }
}
