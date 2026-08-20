using QualityInspectionTracker.Application.Constants;
using QualityInspectionTracker.Application.DTOs;
using QualityInspectionTracker.Application.Interfaces;
using QualityInspectionTracker.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Application.Services
{
    public class InspectionService : IInspectionService
    {
        private readonly IInspectionRepository _repository;

        public InspectionService(
            IInspectionRepository repository)
        {
            _repository = repository;
        }

        public async Task<InspectionDto> CreateAsync(
            CreateInspectionRequest request,
            CancellationToken cancellationToken = default)
        {
            ValidateDefectType(request.DefectType);
            ValidateSeverity(request.Severity);

            var inspection = new Inspection
            {
                InspectionDate = request.InspectionDate,
                MachineLineId = request.MachineLineId.Trim(),
                DefectType = request.DefectType,
                Severity = request.Severity,
                Remarks = request.Remarks?.Trim(),

                Status = InspectionConstants.Statuses.Open,
                Source = InspectionConstants.Sources.Manual,

                CreatedAt = DateTime.UtcNow
            };

            var result = await _repository.AddAsync(
                inspection,
                cancellationToken);

            return MapToDto(result);
        }

        public async Task<InspectionDto?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var inspection = await _repository.GetByIdAsync(
                id,
                cancellationToken);

            return inspection == null
                ? null
                : MapToDto(inspection);
        }

        public async Task<List<InspectionDto>> GetAllAsync(
            InspectionFilterRequest request,
            CancellationToken cancellationToken = default)
        {
            var inspections = await _repository.GetAllAsync(
                request.Severity,
                request.Status,
                request.FromDate,
                request.ToDate,
                request.SortBy,
                request.SortDescending,
                cancellationToken);

            return inspections
                .Select(MapToDto)
                .ToList();
        }

        public async Task<InspectionDto> ResolveAsync(
            int id,
            ResolveInspectionRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(
                request.ResolutionNote))
            {
                throw new ArgumentException(
                    "Resolution note is required.");
            }

            var inspection = await _repository.GetByIdAsync(
                id,
                cancellationToken);

            if (inspection == null)
            {
                throw new KeyNotFoundException(
                    $"Inspection with ID {id} was not found.");
            }

            if (inspection.Status ==
                InspectionConstants.Statuses.Resolved)
            {
                throw new InvalidOperationException(
                    "Inspection is already resolved.");
            }

            inspection.Status =
                InspectionConstants.Statuses.Resolved;

            inspection.ResolutionNote =
                request.ResolutionNote.Trim();

            inspection.ResolvedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(
                inspection,
                cancellationToken);

            return MapToDto(inspection);
        }

        private static InspectionDto MapToDto(
            Inspection inspection)
        {
            return new InspectionDto
            {
                Id = inspection.Id,
                InspectionDate = inspection.InspectionDate,
                MachineLineId = inspection.MachineLineId,
                DefectType = inspection.DefectType,
                Severity = inspection.Severity,
                Remarks = inspection.Remarks,
                Status = inspection.Status,
                ResolutionNote = inspection.ResolutionNote,
                ResolvedAt = inspection.ResolvedAt,
                CreatedAt = inspection.CreatedAt,
                Source = inspection.Source,
                CreatedBy = inspection.CreatedByUser?.DisplayName
            };
        }

        private static void ValidateDefectType(
            string defectType)
        {
            var valid = new[]
            {
            InspectionConstants.DefectTypes.WeaveDefect,
            InspectionConstants.DefectTypes.ShadeVariation,
            InspectionConstants.DefectTypes.HoleTear,
            InspectionConstants.DefectTypes.CountDeviation,
            InspectionConstants.DefectTypes.Other
        };

            if (!valid.Contains(defectType))
            {
                throw new ArgumentException(
                    "Invalid defect type.");
            }
        }

        private static void ValidateSeverity(
            string severity)
        {
            var valid = new[]
            {
            InspectionConstants.Severities.Critical,
            InspectionConstants.Severities.Major,
            InspectionConstants.Severities.Minor
        };

            if (!valid.Contains(severity))
            {
                throw new ArgumentException(
                    "Invalid severity.");
            }
        }
    }
}
