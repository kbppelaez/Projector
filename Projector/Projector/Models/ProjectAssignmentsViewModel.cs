using Projector.Core;
using Projector.Core.Persons;
using Projector.Core.Projects;
using Projector.Data;

namespace Projector.Models
{
    public class ProjectAssignmentsViewModel
    {
        private readonly IProjectsService _projectsService;

        /* CONSTRUCTORS */
        public ProjectAssignmentsViewModel() { }

        public ProjectAssignmentsViewModel(IProjectsService projectsService) {
            _projectsService = projectsService;
        }

        /* PROPERTIES */
        public bool Exists {  get; set; }
        public ProjectData Details { get; set; }
        public List<PersonData> Assignees { get; set; }
        public List<PersonData> UnassignedEmployees { get; set; }

        /* METHODS */
        public async Task Initialize(int projectId)
        {
            Details = await _projectsService.GetProject(projectId);
            if(Details == null)
            {
                Exists = false;
                return;
            }


        }
    }
}
