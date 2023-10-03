using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projector.Data
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [Unicode(false)]
        [StringLength(200, MinimumLength = 5)]
        public string UserName { get; set; }

        [Required]
        [Unicode(false)]
        [StringLength(512)]
        public string Password { get; set; }

        public virtual Person Person { get; set; }
        public bool IsVerified { get; set; } = false;
    }
}
