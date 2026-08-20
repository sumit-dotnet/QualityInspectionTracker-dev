using QualityInspectionTracker.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Application.Interfaces
{
    public interface IInspectionService
    {
        Task<InspectionDto> CreateAsync(
            CreateInspectionRequest request,
            CancellationToken cancellationToken = default);

        Task<InspectionDto?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<List<InspectionDto>> GetAllAsync(
            InspectionFilterRequest request,
            CancellationToken cancellationToken = default);

        Task<InspectionDto> ResolveAsync(
            int id,
            ResolveInspectionRequest request,
            CancellationToken cancellationToken = default);
    }
}
