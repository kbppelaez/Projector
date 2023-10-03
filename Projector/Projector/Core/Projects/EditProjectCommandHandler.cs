using Projector.Core.Projects.DTO;
using Projector.Data;
using System.Globalization;

namespace Projector.Core.Projects
{
    public class EditProjectCommandHandler : ICommandHandler<EditProjectCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IProjectsService _projectsService;

        public EditProjectCommandHandler(IProjectsService projectsService, ProjectorDbContext context) {
            _db = context;
            _projectsService = projectsService;
        }

        public async Task<CommandResult> Execute(EditProjectCommand project)
        {
            Project toEdit = _db.Projects
                    .Where(p => p.Id == project.Project.Id)
                    .Where(p => p.Assignees
                                    .Any(e => e.Id == project.PersonId))
                    .FirstOrDefault();

            if (toEdit == null)
            {
                return CommandResult.Error("404");
            }

            project.Project.Code = project.Project.Code.ToUpper();

            if(!isSame(toEdit.Name, project.Project.Name))
            {
                var duplicate = _db.Projects.ProjectNameExists(project.Project.Name);
                if (duplicate)
                {
                    return CommandResult.Error("Duplicate Project Name given.");
                }

                toEdit.Name = project.Project.Name;
            }

            if (!isSame(toEdit.Code, project.Project.Code))
            {
                var duplicate = _db.Projects.CodeExists(project.Project.Code);
                if (duplicate)
                {
                    return CommandResult.Error("Duplicate Project Code given.");
                }

                toEdit.Code = project.Project.Code;
            }

            try
            {
                project.Project.Budget = decimal.Parse(project.Project.BudgetString, CultureInfo.InvariantCulture);
            }
            catch (Exception) {
                return CommandResult.Error("Invalid Budget Value given.");
            }
            
            toEdit.Budget = project.Project.Budget;
            toEdit.Remarks = project.Project.Remarks;

            _db.Projects.Update(toEdit);
            await _db.SaveChangesAsync();

            return CommandResult.Success(project);
        }

        private bool isSame(string s1, string s2)
        {
            return s1.Equals(s2);
        }
    }
}
