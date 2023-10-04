using Projector.Core;
using Projector.Core.Persons.DTO;

namespace Projector.Models
{
    public class PersonsListViewModel
    {
        private readonly IPersonsService _personsService;

        public PersonsListViewModel() { }

        public PersonsListViewModel(IPersonsService personsService) {
            _personsService = personsService;
            Results = new PersonSearchResult();
        }

        public PersonSearchResult Results { get; set; }
        public PersonSearchQuery Args {  get; set; }
        public int PersonId {  get; set; }
        
        /* for Pagination */
        public List<Dictionary<string,string>> PageArgs { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage {  get; set; }
        public int TotalPages {  get; set; }

        /* METHODS */

        public async Task Initialize(PersonSearchQuery query, int personId)
        {
            Args = query;
            PersonId = personId;
            Results = await _personsService.GetPersons(query);

            PageArgs = Results.TotalPersons > 0 ?
                initializePageTracker() :
                new List<Dictionary<string,string>>();
        }

        private List<Dictionary<string, string>> initializePageTracker()
        {
            TotalPages = computeTotalPages(Args.PageSize, Results.TotalPersons);
            PageArgs = new List<Dictionary<string, string>>();
            int pageCount = 0;

            HasPreviousPage = Args.Page != 0;
            if (HasPreviousPage)
            {
                PageArgs.Add(Args.ToStringArgument(-1)); //-1 since previous page

                //buttons to the left of current page
                for (int left = Args.Page - 2; left != Args.Page; left += 1)
                {
                    if (left >= 0)
                    {
                        PageArgs.Add(Args.ToStringArgument(left - Args.Page));
                        pageCount += 1;
                    }
                }
            }

            //current page
            PageArgs.Add(Args.ToStringArgument(0));
            pageCount += 1;

            HasNextPage = Args.Page != TotalPages - 1;
            if (HasNextPage)
            {
                //buttons to the right of the current page
                int i = 1;
                while (pageCount < 5 && Args.Page + i < TotalPages)
                {
                    PageArgs.Add(Args.ToStringArgument(i++));
                    pageCount += 1;
                }

                PageArgs.Add(Args.ToStringArgument(1)); //next page of current page
            }

            return PageArgs;
        }

        private int computeTotalPages(int pageSize, int totalProjects)
        {
            return (int)Math.Ceiling(totalProjects / (double)pageSize);
        }

        public int GetPageNumber(string page)
        {
            return int.Parse(page) + 1;
        }
    }
}
