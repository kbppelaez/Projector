namespace Projector.Core.Persons.DTO
{
    public class UnassignedPersonsData
    {
        public UnassignedPersonsData() { }

        public PersonData[] Unassigned {  get; set; }

        public int UnassignedProjectId { get; set; }
    }
}
