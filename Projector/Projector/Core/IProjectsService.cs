using Projector.Core.Projects;
using Projector.Core.Projects.DTO;

namespace Projector.Core
{
    public interface IProjectsService
    {
        Task<ProjectData[]> GetPersonProjects(ProjectSearchQuery args);
        Task<ProjectDetailsData> GetProjectDetails(int projectId);
    }
}
