using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QualityInspectionTracker.Application.Interfaces;

namespace QualityInspectionTracker.API.Controllers
{
    [ApiController]
    [Route("api/summary")]
    [Authorize]
    public class SummaryController : ControllerBase
    {
        private readonly IInspectionRepository _repository;

        public SummaryController(
            IInspectionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            CancellationToken cancellationToken)
        {
            var inspections =
                await _repository.GetAllForSummaryAsync(
                    cancellationToken);

            var result = new
            {
                Critical = new
                {
                    Open = inspections.Count(x =>
                        x.Severity == "Critical" &&
                        x.Status == "Open"),

                    Resolved = inspections.Count(x =>
                        x.Severity == "Critical" &&
                        x.Status == "Resolved")
                },

                Major = new
                {
                    Open = inspections.Count(x =>
                        x.Severity == "Major" &&
                        x.Status == "Open"),

                    Resolved = inspections.Count(x =>
                        x.Severity == "Major" &&
                        x.Status == "Resolved")
                },

                Minor = new
                {
                    Open = inspections.Count(x =>
                        x.Severity == "Minor" &&
                        x.Status == "Open"),

                    Resolved = inspections.Count(x =>
                        x.Severity == "Minor" &&
                        x.Status == "Resolved")
                }
            };

            return Ok(result);
        }
    }
}
