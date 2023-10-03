using Projector.Core.Persons.DTO;

namespace Projector.Core.Projects.DTO
{
    public class ProjectDetailsData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public string BudgetString { get; set; }
        public string Remarks { get; set; }
        public PersonData[] Assigned { get; set; }
    }
}
