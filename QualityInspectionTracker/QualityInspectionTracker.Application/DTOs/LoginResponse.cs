using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Application.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;

        public int UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
    }
}
