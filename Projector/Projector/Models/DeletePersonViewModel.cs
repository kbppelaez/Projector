using Projector.Core;
using Projector.Core.Persons.DTO;

namespace Projector.Models
{
    public class DeletePersonViewModel
    {
        private readonly IPersonsService _personsService;

        public DeletePersonViewModel() { }
        public DeletePersonViewModel(IPersonsService personsService) {
            _personsService = personsService;
        }

        /* PROPERTIES */
        public PersonData Person { get; set; }

        public bool PersonExists { get; set; }

        /* METHODS */

        public async Task Initialize(int personId)
        {
            Person = await _personsService.GetPerson(personId);
            PersonExists = Person != null;
        }
    }
}
