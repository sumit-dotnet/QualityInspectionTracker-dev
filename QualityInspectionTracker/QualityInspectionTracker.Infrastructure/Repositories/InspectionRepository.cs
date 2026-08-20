using Microsoft.EntityFrameworkCore;
using QualityInspectionTracker.Application.Interfaces;
using QualityInspectionTracker.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Infrastructure.Repositories
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly AppDbContext _context;

        public InspectionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Inspection> AddAsync(
            Inspection inspection,
            CancellationToken cancellationToken = default)
        {
            await _context.Inspections.AddAsync(
                inspection,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return inspection;
        }

        public async Task<Inspection?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Inspections
                .Include(x => x.CreatedByUser)
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

        public async Task<List<Inspection>> GetAllAsync(
            string? severity,
            string? status,
            DateOnly? fromDate,
            DateOnly? toDate,
            string? sortBy,
            bool sortDescending,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Inspection> query =
                _context.Inspections
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser);

            if (!string.IsNullOrWhiteSpace(severity))
            {
                query = query.Where(
                    x => x.Severity == severity);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(
                    x => x.Status == status);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(
                    x => x.InspectionDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(
                    x => x.InspectionDate <= toDate.Value);
            }

            query = sortBy?.ToLowerInvariant() switch
            {
                "severity" => sortDescending
                    ? query.OrderByDescending(x => x.Severity)
                    : query.OrderBy(x => x.Severity),

                "status" => sortDescending
                    ? query.OrderByDescending(x => x.Status)
                    : query.OrderBy(x => x.Status),

                "machinelineid" => sortDescending
                    ? query.OrderByDescending(x => x.MachineLineId)
                    : query.OrderBy(x => x.MachineLineId),

                _ => sortDescending
                    ? query.OrderByDescending(x => x.InspectionDate)
                    : query.OrderBy(x => x.InspectionDate)
            };

            return await query.ToListAsync(
                cancellationToken);
        }

        public async Task UpdateAsync(
            Inspection inspection,
            CancellationToken cancellationToken = default)
        {
            _context.Inspections.Update(inspection);

            await _context.SaveChangesAsync(
                cancellationToken);
        }

        public async Task<List<Inspection>> GetAllForSummaryAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Inspections
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
