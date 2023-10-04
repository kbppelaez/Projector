namespace Projector.Core.Persons.DTO
{
    public class PersonSearchQuery
    {
        public PersonSearchQuery() { }

        /* PROPERTIES */
        public string Term { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; } = 5;

        public int ViewPage
        {
            get
            {
                return Page + 1;
            }
        }

        /* METHODS */
        public Dictionary<string, string> ToStringArgument(int change)
        {
            return new Dictionary<string, string>
            {
                { "Term", this.Term },
                { "Page", (this.Page + change).ToString() },
                { "PageSize", this.PageSize.ToString() }
            };
        }
    }
}
