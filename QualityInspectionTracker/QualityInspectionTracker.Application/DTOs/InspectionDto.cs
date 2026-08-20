using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Application.DTOs
{
    public class InspectionDto
    {
        public int Id { get; set; }

        public DateOnly InspectionDate { get; set; }

        public string MachineLineId { get; set; } = string.Empty;

        public string DefectType { get; set; } = string.Empty;

        public string Severity { get; set; } = string.Empty;

        public string? Remarks { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? ResolutionNote { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Source { get; set; } = string.Empty;

        public string? CreatedBy { get; set; }
    }
}
