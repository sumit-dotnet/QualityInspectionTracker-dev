using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Application.DTOs
{
    public class SupervisorDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
