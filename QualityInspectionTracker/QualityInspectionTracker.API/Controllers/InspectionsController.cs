using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QualityInspectionTracker.Application.DTOs;
using QualityInspectionTracker.Application.Interfaces;

namespace QualityInspectionTracker.API.Controllers
{
    [ApiController]
    [Route("api/Inspections")]
    [Authorize]
    public class InspectionsController : ControllerBase
    {
        private readonly IInspectionService _service;

        public InspectionsController(
            IInspectionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateInspectionRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] InspectionFilterRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _service.GetAllAsync(
                request,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _service.GetByIdAsync(
                id,
                cancellationToken);

            if (result == null)
            {
                return NotFound(new
                {
                    message = $"Inspection {id} not found."
                });
            }

            return Ok(result);
        }

        [HttpPut("{id:int}/resolve")]
        public async Task<IActionResult> Resolve(
            int id,
            ResolveInspectionRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.ResolveAsync(
                    id,
                    request,
                    cancellationToken);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
