using System.ComponentModel.DataAnnotations;

namespace Projector.Core.Users.DTO
{
    public class NewUserData
    {
        /* PROPERTIES */
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(11, MinimumLength = 7, ErrorMessage = "Password should be 7-11 characters only.")]
        [RegularExpression(@"^[!-~]{7,11}$", ErrorMessage = "Please input alphanumeric or special characters only.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(11, MinimumLength = 7, ErrorMessage = "Password should be 7-11 characters only.")]
        [RegularExpression(@"^[!-~]{7,11}$", ErrorMessage = "Please input alphanumeric or special characters only.")]
        public string ConfirmPassword { get; set; }

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
