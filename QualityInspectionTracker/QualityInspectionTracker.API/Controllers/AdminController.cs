using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QualityInspectionTracker.Application.DTOs;
using QualityInspectionTracker.Application.Interfaces;

namespace QualityInspectionTracker.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("supervisors")]
        public async Task<IActionResult> CreateSupervisor(
            CreateSupervisorRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var result =
                    await _adminService.CreateSupervisorAsync(
                        request,
                        cancellationToken);

                return Created(
                    $"/api/admin/supervisors/{result.Id}",
                    result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("supervisors")]
        public async Task<IActionResult> GetSupervisors(
            CancellationToken cancellationToken)
        {
            var result =
                await _adminService.GetSupervisorsAsync(
                    cancellationToken);

            return Ok(result);
        }
    }
}
