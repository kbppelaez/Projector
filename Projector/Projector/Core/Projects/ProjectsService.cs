using Microsoft.EntityFrameworkCore;
using Projector.Core.Persons;
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

            data.Assignees = await getAssignees(project);
            data.UnassignedEmployees = await getUnassignedEmployees(project);

            return data;
        }

        private async Task<List<PersonData>> getAssignees(Project project)
        {
            var personsQuery = _db.Persons
                .Where(
                    p => p.Projects
                        .Any(proj => proj.Id == project.Id)
                );

            return await personsQuery
                .Select(p => new PersonData
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    UserId = p.UserId,
                })
                .ToListAsync();
        }
        private async Task<List<PersonData>> getUnassignedEmployees(Project project)
        {
            var personsQuery = _db.Persons
                .Where(
                    p => p.Projects
                        .All(proj => proj.Id != project.Id)
                );

            return await personsQuery
                .Select(p => new PersonData
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    UserId = p.UserId,
                })
                .ToListAsync();
        }

    }
}
