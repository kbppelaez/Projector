namespace Projector.Core.Projects.DTO
{
    public class ProjectSearchQuery
    {
        public string Term { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; } = 2;
        public int PersonId { get; set; }
        public int ViewPage
        {
            get
            {
                return Page + 1;
            }
        }

        public Dictionary<string, string> ToStringArgument(int change)
        {
            return new Dictionary<string, string>
            {
                {"Term", this.Term },
                {"Page", ((this.Page) + change).ToString() },
                {"PageSize", this.PageSize.ToString()}
            };
        }
    }
}
