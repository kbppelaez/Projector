namespace Projector.Core.Projects.DTO
{
    public class AssignPersonCommand
    {
        public AssignPersonCommand() { }
        public AssignPersonCommand(AssigneeData assigneeData) {
            PersonId = assigneeData.PersonId;
            ProjectId = assigneeData.ProjectId;
        }

        public int PersonId { get; set; }

        public int ProjectId { get; set; }

    }
}
