using Projector.Data;
using System.ComponentModel.DataAnnotations;

namespace Projector.Core.Persons.DTO
{
    public class PersonData
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        public int UserId { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public PersonData() { }

        public PersonData(Person person)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            UserId = person.UserId;
        }
    }
}
