using Projector.Core;
using Projector.Core.Persons.DTO;

namespace Projector.Models
{
    public class EditPersonViewModel
    {
        private readonly IPersonsService _personsService;

        public EditPersonViewModel() { }

        public EditPersonViewModel(IPersonsService personsService) { 
            _personsService = personsService;
            Errors = Enumerable.Empty<string>();
        }

        public EditPersonViewModel(NewPersonData edited, IEnumerable<string> errors)
        {
            EditableValues = edited;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        /* Properties */
        public NewPersonData EditableValues { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public bool PersonExists { get; set; }
        public int PersonId { get; set; }

        public async Task Initialize(int personId)
        {
            PersonData person = await _personsService.GetPerson(personId);
            PersonExists = person != null;

            if (!PersonExists)
                return;
            PersonId = personId;
            EditableValues = createNewPersonData(person);
        }

        private NewPersonData createNewPersonData(PersonData person)
        {
            return new NewPersonData
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                UserName = person.User.UserName
            };
        }
    }
}
