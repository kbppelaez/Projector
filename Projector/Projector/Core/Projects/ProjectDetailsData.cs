using Projector.Core.Persons;

namespace Projector.Core.Projects
{
    public class ProjectDetailsData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public string BudgetString { get; set; }
        public string Remarks { get; set; }
        public List<PersonData> Assignees { get; set; }
        public List<PersonData> UnassignedEmployees { get; set; }
    }
}
