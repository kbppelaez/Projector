using Projector.Core;
using Projector.Core.Projects.DTO;

namespace Projector.Models
{
    public class EditProjectViewModel
    {
        private readonly IProjectsService _projectsService;

        public EditProjectViewModel() { }

        public EditProjectViewModel(IProjectsService projectsService) {
            _projectsService = projectsService;
            Errors = Enumerable.Empty<string>();
        }

        public EditProjectViewModel(ProjectData project, IEnumerable<string> errors)
        {
            Project = project;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public ProjectData Project { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public bool ProjectExists { get; set; }

        public async Task Initialize(int projectId, int personId)
        {
            Project = await _projectsService.GetProjectDetails(projectId, personId);
            ProjectExists = Project != null;
        }
    }
}
