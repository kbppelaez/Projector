namespace Projector.Core.Projects.DTO
{
    public class ProjectSearchQuery
    {
        public string Term { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PersonId { get; set; }
        public int ViewPage
        {
            get
            {
                return Page + 1;
            }
        }
    }
}
