using System.ComponentModel.DataAnnotations;

namespace Projector.Core.Users.DTO
{
    public class UserValidationData
    {
        public int UserId { get; set; }
        public string Token { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
