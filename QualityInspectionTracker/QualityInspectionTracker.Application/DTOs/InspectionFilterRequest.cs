using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Application.DTOs
{
    public class InspectionFilterRequest
    {
        public string? Severity { get; set; }

        public string? Status { get; set; }

        public DateOnly? FromDate { get; set; }

        public DateOnly? ToDate { get; set; }

        public string? SortBy { get; set; }

        public bool SortDescending { get; set; } = true;
    }
}
