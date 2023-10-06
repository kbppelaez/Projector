using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Projector.Data
{
    public class VerificationLink
    {
        public int Id { get; set; }

        [Required]
        [Unicode(false)]
        [StringLength(512)]
        public string ActivationToken { get; set; }

        [Required]
        [Unicode(false)]
        [StringLength(512)]
        public string ActivationLink { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }


        public virtual User User { get; set; }
    }
}
