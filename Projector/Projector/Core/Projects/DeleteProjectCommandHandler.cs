using Microsoft.EntityFrameworkCore;
using Projector.Core.Projects.DTO;
using Projector.Data;
using System.Collections;

namespace Projector.Core.Projects
{
    public class DeleteProjectCommandHandler : ICommandHandler<DeleteProjectCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IProjectsService _projectsService;

        public DeleteProjectCommandHandler() { }

        public DeleteProjectCommandHandler(ProjectorDbContext context, IProjectsService projectsService) {
            _db = context;
            _projectsService = projectsService;
        }

        public async Task<CommandResult> Execute(DeleteProjectCommand cmd)
        {
            if(cmd == null)
            {
                return CommandResult.Error("404");
            }

            /* IF PersonProject entry should be deleted */
            /*
            Project project = await _db.Projects.Include(p => p.Assignees)
                .Where(p => p.Id == cmd.Project.Id)
                .FirstOrDefaultAsync();
            //delete dependencies first
            project.Assignees.Clear();
            */

            /* IF PersonProject entry should NOT be deletd */
            Project project = await _db.Projects.FindAsync(cmd.Project.Id);
            project.IsDeleted = true;
            _db.Projects.Update(project);

            await _db.SaveChangesAsync();

            return CommandResult.Success(cmd.Project);
        }
    }
}
