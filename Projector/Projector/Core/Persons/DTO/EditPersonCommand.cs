namespace Projector.Core.Persons.DTO
{
    public class EditPersonCommand
    {
        public NewPersonData EditedPerson {  get; set; }
        public int PersonId {  get; set; }
    }
}
