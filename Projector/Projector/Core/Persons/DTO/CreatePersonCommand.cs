namespace Projector.Core.Persons.DTO
{
    public class CreatePersonCommand
    {
        public NewPersonData NewPerson {  get; set; }
        public string CreateNewPasswordBaseUrl { get; set; }
    }
}
