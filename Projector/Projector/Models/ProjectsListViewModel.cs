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
            TotalProjects = Projects.Length;

            PageArgs = TotalProjects > 0 ?
                initializePageTracker() :
                new List<Dictionary<string, string>>();
        }

        private List<Dictionary<string,string>> initializePageTracker()
        {
            TotalPages = computeTotalPages(Args.PageSize, TotalProjects);
            PageArgs = new List<Dictionary<string, string>>();
            int pageCount = 0;

            HasPreviousPage = Args.Page == 0 ? false : true;
            if (HasPreviousPage)
            {
                PageArgs.Add(Args.ToStringArgument(-1)); //-1 since previous page
            }

            //buttons to the left of current page
            for(int left = Args.Page-2; left!=Args.Page; left += 1)
            {
                if (left >= 0)
                {
                    PageArgs.Add(Args.ToStringArgument(left - Args.Page));
                    pageCount += 1;
                }
            }

            //current page
            PageArgs.Add(Args.ToStringArgument(0));
            pageCount += 1;

            //buttons to the right of the current page
            int i = 1;
            while(pageCount < 5 && Args.Page+i < TotalPages)
            {
                PageArgs.Add(Args.ToStringArgument(i++));
                pageCount += 1;
            }

            HasNextPage = Args.Page == TotalPages - 1 ? false : true;
            if (HasNextPage)
            {
                PageArgs.Add(Args.ToStringArgument(1)); //next page of current page
            }

            return PageArgs;
        }

        private int computeTotalPages(int pageSize, int totalProjects)
        {
            return (int)Math.Ceiling(totalProjects / (double)pageSize);
        }
    }
}
