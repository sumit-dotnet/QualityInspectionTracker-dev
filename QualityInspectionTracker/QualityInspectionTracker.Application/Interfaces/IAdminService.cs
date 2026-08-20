using QualityInspectionTracker.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Application.Interfaces
{
    public interface IAdminService
    {
        Task<SupervisorDto> CreateSupervisorAsync(
            CreateSupervisorRequest request,
            CancellationToken cancellationToken = default);

        Task<List<SupervisorDto>> GetSupervisorsAsync(
            CancellationToken cancellationToken = default);
    }
}
