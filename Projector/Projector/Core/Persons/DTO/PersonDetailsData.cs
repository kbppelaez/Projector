using Projector.Core.Projects.DTO;
using Projector.Core.Users.DTO;
using System.ComponentModel.DataAnnotations;

namespace Projector.Core.Persons.DTO
{
    public class PersonDetailsData
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserData User { get; set; }
        public ProjectData[] Projects { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
