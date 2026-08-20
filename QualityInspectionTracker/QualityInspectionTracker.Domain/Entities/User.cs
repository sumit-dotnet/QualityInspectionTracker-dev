using System;
using System.Collections.Generic;

namespace QualityInspectionTracker.Infrastructure;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
}
