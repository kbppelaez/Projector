using Projector.Core.Persons.DTO;
using Projector.Core.Projects.DTO;

namespace Projector.Core
{
    public interface IProjectsService
    {
        Task<ProjectSearchResult> GetPersonProjects(ProjectSearchQuery args);
        Task<ProjectDetailsData> GetProjectDetails(int projectId);
        Task<ProjectData> GetProjectDetails(int projectId, int personId);
        Task<PersonData[]> GetAssigned(int projectId);
        Task<PersonData[]> GetUnassigned(int projectId);
        Task<string> AssignPersonToProject(int personId, int projectId);
        Task<string> RemovePersonFromProject(int personId, int projectId);
    }
}
