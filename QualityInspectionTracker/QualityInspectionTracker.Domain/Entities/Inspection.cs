using System;
using System.Collections.Generic;

namespace QualityInspectionTracker.Infrastructure;

public partial class Inspection
{
    public int Id { get; set; }

    public DateOnly InspectionDate { get; set; }

    public string MachineLineId { get; set; } = null!;

    public string DefectType { get; set; } = null!;

    public string Severity { get; set; } = null!;

    public string? Remarks { get; set; }

    public string Status { get; set; } = null!;

    public string? ResolutionNote { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Source { get; set; } = null!;

    public int? CreatedByUserId { get; set; }

    public virtual User? CreatedByUser { get; set; }
}
