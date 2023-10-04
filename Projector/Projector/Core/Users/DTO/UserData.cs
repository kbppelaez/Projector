using Projector.Data;
using System.ComponentModel.DataAnnotations;

namespace Projector.Core.Users.DTO
{
    public class UserData
    {
        public UserData() { }
        public UserData(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Password = string.Empty;
            IsVerified = user.IsVerified;
        }

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

        public bool IsVerified {  get; set; }

    }
}
