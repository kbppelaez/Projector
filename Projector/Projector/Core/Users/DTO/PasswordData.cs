using System.ComponentModel.DataAnnotations;

namespace Projector.Core.Users.DTO
{
    public class PasswordData
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(11, MinimumLength = 7, ErrorMessage = "Password should be 7-11 characters only.")]
        [RegularExpression(@"^[!-~]{7,11}$", ErrorMessage = "Please input alphanumeric or special characters only.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [StringLength(11, MinimumLength = 7, ErrorMessage = "Password should be 7-11 characters only.")]
        [RegularExpression(@"^[!-~]{7,11}$", ErrorMessage = "Please input alphanumeric or special characters only.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public int Status { get; set; }

    }
}
