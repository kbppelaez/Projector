using Projector.Core.Projects.DTO;
using Projector.Data;

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

            //delete dependencies first
            Project project = await _db.Projects.FindAsync(cmd.Project.Id);
            project.Assignees.Clear();
            _db.Projects.Update(project);

            //delete project
            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();

            return CommandResult.Success(cmd.Project);
        }
    }
}
