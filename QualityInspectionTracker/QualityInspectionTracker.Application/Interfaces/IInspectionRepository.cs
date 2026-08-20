using QualityInspectionTracker.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Application.Interfaces
{
    public interface IInspectionRepository
    {
        Task<Inspection> AddAsync(
            Inspection inspection,
            CancellationToken cancellationToken = default);

        Task<Inspection?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<List<Inspection>> GetAllAsync(
            string? severity,
            string? status,
            DateOnly? fromDate,
            DateOnly? toDate,
            string? sortBy,
            bool sortDescending,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
            Inspection inspection,
            CancellationToken cancellationToken = default);

        Task<List<Inspection>> GetAllForSummaryAsync(
            CancellationToken cancellationToken = default);
    }
}
