using Microsoft.EntityFrameworkCore;
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

        public async Task<ProjectData[]> GetPersonProjects(ProjectSearchQuery args)
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

            return await projectsQuery
                .Select(proj => new ProjectData
                {
                    Id = proj.Id,
                    Code = proj.Code,
                    Name = proj.Name,
                    Budget = proj.Budget,
                    Remarks = proj.Remarks
                })
                .ToArrayAsync();
        }

        public async Task<ProjectData> GetProject(int projectId)
        {
            Project project = await _db.Projects
                .Where(p => p.Id == projectId)
                .FirstOrDefaultAsync();

            return project == null ?
                null :
                new ProjectData
                {
                    Id = project.Id,
                    Code = project.Code,
                    Name = project.Name,
                    Budget = project.Budget,
                    Remarks = project.Remarks
                };
        }
    }
}
