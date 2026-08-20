using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QualityInspectionTracker.Application.DTOs
{
    public class CreateInspectionRequest
    {
        [Required]
        public DateOnly InspectionDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string MachineLineId { get; set; } = string.Empty;

        [Required]
        public string DefectType { get; set; } = string.Empty;

        [Required]
        public string Severity { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Remarks { get; set; }
    }
}
