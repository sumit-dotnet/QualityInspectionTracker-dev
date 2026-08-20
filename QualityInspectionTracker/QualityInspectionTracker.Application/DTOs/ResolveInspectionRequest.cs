using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QualityInspectionTracker.Application.DTOs
{
    public class ResolveInspectionRequest
    {
        [Required]
        [MaxLength(1000)]
        public string ResolutionNote { get; set; } = string.Empty;
    }
}
