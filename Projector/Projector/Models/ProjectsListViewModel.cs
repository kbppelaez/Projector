using Projector.Core;
using Projector.Core.Projects;
using Projector.Core.Projects.DTO;
using System.Security.Claims;

namespace Projector.Models
{
    public class ProjectsListViewModel
    {
        private readonly IProjectsService _projectsService;

        /* CONSTRUCTOR */
        public ProjectsListViewModel() { }
        public ProjectsListViewModel(IProjectsService projectsService) { 
            _projectsService = projectsService;
            Projects = Array.Empty<ProjectData>();
        }

        /* PROPERTIES */
        //for Content
        public ProjectData[] Projects { get; set; }
        public ProjectSearchQuery Args { get; set; }

        //for Pagination
        public List<Dictionary<string,string>> PageArgs { get; set; }
        public int TotalProjects { get; set; }
        public int TotalPages {  get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        /* METHODS */
        public async Task Initialize(ProjectSearchQuery query)
        {
            Projects = await _projectsService.GetPersonProjects(query);
        }
    }
}
