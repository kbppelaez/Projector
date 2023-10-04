using Projector.Core.Projects.DTO;
using Projector.Data;
using System.Globalization;

namespace Projector.Core.Projects
{
    public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IProjectsService _projectsService;

        public CreateProjectCommandHandler(ProjectorDbContext context, IProjectsService projectsService) {
            _db = context;
            _projectsService = projectsService;
        }
        public async Task<CommandResult> Execute(CreateProjectCommand newProject)
        {
            if(string.IsNullOrEmpty(newProject.Project.Name)
                || string.IsNullOrEmpty(newProject.Project.Code))
            {
                return CommandResult.Error("Invalid Project Name or Code given.");
            }

            newProject.Project.Code = newProject.Project.Code.ToUpper();

            var duplicateCodeName = _db.Projects.CodeOrProjectNameExists(newProject.Project.Name, newProject.Project.Code);
            if (duplicateCodeName)
            {
                return CommandResult.Error("Duplicate Project Name or Code given.");
            }

            try
            {
                newProject.Project.Budget = decimal.Parse(newProject.Project.BudgetString, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return CommandResult.Error("Invalid Budget Value given.");
            }

            Person creator = _db.Persons.FirstOrDefault(p => p.Id == newProject.PersonId);

            Project project = new Project {
                Code = newProject.Project.Code,
                Name = newProject.Project.Name,
                Budget = newProject.Project.Budget,
                Remarks = newProject.Project.Remarks,
                IsDeleted = false,
                Assignees = new List<Person> { creator }
            };

            _db.Projects.Add(project);
            await _db.SaveChangesAsync();

            return CommandResult.Success(project.Id);
        }
    }
}
