using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
        [StringLength(11, MinimumLength = 7)]
        public string Password { get; set; }

        /*
        public bool isVerified { get; set; }
         
        public string linkHash { get; set; }

        public int PersonID { get; set; }
         */
    }
}
