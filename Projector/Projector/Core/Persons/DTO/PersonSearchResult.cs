namespace Projector.Core.Persons.DTO
{
    public class PersonSearchResult
    {
        public PersonSearchResult() { }

        public PersonData[] People { get; set; }

        public int TotalPersons { get; set; }
    }
}
