using System.ComponentModel.DataAnnotations;

namespace Projector.Core.Users.DTO
{
    public class UserData
    {
        /* PROPERTIES */
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
