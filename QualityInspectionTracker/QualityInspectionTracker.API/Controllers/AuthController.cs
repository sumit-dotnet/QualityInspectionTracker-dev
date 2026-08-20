using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QualityInspectionTracker.Application.DTOs;
using QualityInspectionTracker.Application.Interfaces;

namespace QualityInspectionTracker.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(
                request,
                cancellationToken);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password."
                });
            }

            return Ok(result);
        }
    }
}
