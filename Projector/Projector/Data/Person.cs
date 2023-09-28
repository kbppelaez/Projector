using System.ComponentModel.DataAnnotations;

namespace Projector.Data
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
