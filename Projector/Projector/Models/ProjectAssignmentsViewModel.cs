using Projector.Core;
using Projector.Core.Persons.DTO;
using Projector.Core.Projects.DTO;

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
        public bool ProjectExists {  get; set; }
        public ProjectDetailsData Details { get; set; }
        public UnassignedPersonsData People { get; set; }

        /* METHODS */
        public async Task Initialize(int projectId)
        {
            Details = await _projectsService.GetProjectDetails(projectId);
            ProjectExists = Details != null;

            People = new UnassignedPersonsData
            {
                Unassigned = ProjectExists ?
                    await _projectsService.GetUnassigned(projectId) :
                    Array.Empty<PersonData>(),
                UnassignedProjectId = projectId
            };
            
        }
    }
}
