using Microsoft.EntityFrameworkCore;
using Projector.Core.Persons.DTO;
using Projector.Core.Projects.DTO;
using Projector.Data;

namespace Projector.Core.Projects
{
    public class ProjectsService : IProjectsService
    {
        private readonly ProjectorDbContext _db;

        public ProjectsService(ProjectorDbContext db) {
            _db = db;
        }

        public async Task<ProjectSearchResult> GetPersonProjects(ProjectSearchQuery args)
        {
            var projectsQuery = _db.Projects
                .Where(proj => proj.Assignees
                    .Any(person => person.Id == args.PersonId)
                );

            if (!string.IsNullOrEmpty(args.Term))
            {
                projectsQuery = projectsQuery
                    .Where(proj => proj.Name.Contains(args.Term)
                        || proj.Code.Contains(args.Term));
            }

            int totalProjects = projectsQuery.Count();

            projectsQuery = projectsQuery.OrderBy(proj => proj.Code);
            projectsQuery = skipProjects(projectsQuery, args.Page, args.PageSize);

            return new ProjectSearchResult{
                Projects = await projectsQuery
                                .Select(proj => new ProjectData
                                {
                                    Id = proj.Id,
                                    Code = proj.Code,
                                    Name = proj.Name,
                                    Budget = proj.Budget,
                                    Remarks = proj.Remarks
                                })
                                .ToArrayAsync(),
                TotalProjects = totalProjects
            };
        }

        private IQueryable<Project> skipProjects(IQueryable<Project> query, int page, int pageSize)
        {
            return query.Skip(page * pageSize).Take(pageSize);
        }

        public async Task<ProjectDetailsData> GetProjectDetails(int projectId)
        {
            Project project = await _db.Projects
                .Where(p => p.Id == projectId)
                .FirstOrDefaultAsync();

            if(project == null)
            {
                return null;
            }

            ProjectDetailsData data = new ProjectDetailsData
            {
                Id = project.Id,
                Code = project.Code,
                Name = project.Name,
                Budget = project.Budget,
                Remarks = project.Remarks
            };

            data.Assigned = await GetAssigned(project.Id);

            return data;
        }

        public async Task<PersonData[]> GetAssigned(int projectId)
        {
            var personsQuery = _db.Persons
                .Where(p => p.Projects
                        .Any(proj => proj.Id == projectId)
                );

            return await personsQuery
                .Select(p => new PersonData
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    UserId = p.UserId,
                })
                .ToArrayAsync();
        }

        public async Task<PersonData[]> GetUnassigned(int projectId)
        {
            var personsQuery = _db.Persons
                .Where(
                    p => p.Projects
                        .All(proj => proj.Id != projectId)
                );

            return await personsQuery
                .Select(p => new PersonData
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    UserId = p.UserId,
                })
                .ToArrayAsync();
        }

    }
}
