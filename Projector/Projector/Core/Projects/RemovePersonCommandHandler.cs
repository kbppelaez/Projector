using Projector.Core.Projects.DTO;
using Projector.Data;

namespace Projector.Core.Projects
{
    public class RemovePersonCommandHandler : ICommandHandler<RemovePersonCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IProjectsService _projectsService;

        public RemovePersonCommandHandler() { }

        public RemovePersonCommandHandler(ProjectorDbContext db, IProjectsService projectsService)
        {
            _db = db;
            _projectsService = projectsService;
        }

        public async Task<CommandResult> Execute(RemovePersonCommand args)
        {
            var result = await _projectsService.RemovePersonFromProject(args.PersonId, args.ProjectId);

            return result == "OK" ?
                CommandResult.Success(result) :
                CommandResult.Error(result);
        }
    }
}
