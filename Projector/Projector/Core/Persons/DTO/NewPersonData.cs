using System.ComponentModel.DataAnnotations;

namespace Projector.Core.Persons.DTO
{
    public class NewPersonData
    {
        /* PROPERTIES */
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
