using Projector.Core.Users.DTO;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Projector.Models
{
    public class VerifyUserViewModel
    {
        public VerifyUserViewModel() {
            Errors = Enumerable.Empty<string>();
        }

        public VerifyUserViewModel(UserValidationData userData, IEnumerable<string> errors)
        {
            SetValidationData(userData);
            Errors = errors ?? Enumerable.Empty<string>();
        }
        public int UserId { get; set; }
        public string Token { get; set; }

        [Required]
        public string Code { get; set; }

        public IEnumerable<string> Errors { get; set; }

        private void SetValidationData(UserValidationData data)
        {
            UserId = data.UserId;
            Token = data.Token ?? string.Empty;
            Code = data.Code ?? string.Empty;
        }

    }
}
