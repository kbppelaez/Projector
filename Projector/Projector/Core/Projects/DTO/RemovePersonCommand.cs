namespace Projector.Core.Projects.DTO
{
    public class RemovePersonCommand
    {
        public RemovePersonCommand() { }
        public RemovePersonCommand(AssigneeData assigneeData)
        {
            PersonId = assigneeData.PersonId;
            ProjectId = assigneeData.ProjectId;
        }

        public int PersonId { get; set; }

        public int ProjectId { get; set; }
    }
}
