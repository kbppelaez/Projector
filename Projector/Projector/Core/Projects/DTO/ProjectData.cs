﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projector.Core.Projects.DTO
{
    public class ProjectData
    {
        public int? Id { get; set; }

        [Required]
        [RegularExpression(@"^[!-~]{2,50}$", ErrorMessage = "Only basic punctuation marks are allowed, excluding spaces.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Please enter 2-50 characters only.")]
        public string Code { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Please enter 2-50 characters only.")]
        public string Name { get; set; }

        [Precision(18, 4)]
        public decimal Budget { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name="Budget")]
        [Column(TypeName = "decimal(18,4)")]
        public string BudgetString { get; set; }
        public string Remarks { get; set; }
    }
}
