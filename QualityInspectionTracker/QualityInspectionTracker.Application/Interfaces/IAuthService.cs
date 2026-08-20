using QualityInspectionTracker.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default);
    }
}
