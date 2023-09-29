namespace Projector.Core.Projects.DTO
{
    public class ProjectSearchResult
    {
        public ProjectData[] Projects { get; set; }

        public int Count => Projects.Length;
    }
}
