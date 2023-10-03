using Projector.Core;
using Projector.Core.Projects.DTO;

namespace Projector.Models
{
    public class DeleteProjectViewModel
    {
        private readonly IProjectsService _projectService;

        public DeleteProjectViewModel(IProjectsService projectsService) {
            _projectService = projectsService;
        }

        public DeleteProjectViewModel(ProjectData project)
        {
            Project = project;
        }

        public ProjectData Project {  get; set; }
        public bool ProjectExists { get; set; }


        public async Task Initialize(int projectId, int personId)
        {
            Project = await _projectService.GetProjectDetails(projectId, personId);
            ProjectExists = Project != null;
        }
    }
}
