using Projector.Core.Projects.DTO;
using Projector.Data;

namespace Projector.Core.Projects
{
    public class AssignPersonCommandHandler : ICommandHandler<AssignPersonCommand>
    {
        private readonly ProjectorDbContext _db;
        private readonly IProjectsService _projectsService;

        public AssignPersonCommandHandler() { }

        public AssignPersonCommandHandler(ProjectorDbContext db, IProjectsService projectsService) {
            _db = db;
            _projectsService = projectsService;
        }

        public async Task<CommandResult> Execute(AssignPersonCommand args)
        {
            var result = await _projectsService.AssignPersonToProject(args.PersonId, args.ProjectId);

            return result == "OK" ?
                CommandResult.Success(result) :
                CommandResult.Error(result);
        }
    }
}
