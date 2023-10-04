using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Projector.Data
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        [Unicode(false)]
        [RegularExpression(@"^[!-~]{2,50]$")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Please enter 2-50 characters only.")]
        public string Code { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Please enter 2-50 characters only.")]
        public string Name { get; set; }

        [Required]
        [Precision(18,4)]
        public decimal Budget { get; set; }

        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }

        /* NAVIGATION */
        public virtual ICollection<Person> Assignees { get; set; } = new List<Person>();
    }
}
