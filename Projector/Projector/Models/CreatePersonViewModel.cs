using Projector.Core.Persons.DTO;

namespace Projector.Models
{
    public class CreatePersonViewModel
    {
        public CreatePersonViewModel()
        {
            Errors = Enumerable.Empty<string>();
        }

        public CreatePersonViewModel(NewPersonData newPerson, IEnumerable<string> errors)
        {
            NewPerson = newPerson;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public NewPersonData NewPerson { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
