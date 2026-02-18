using Microsoft.AspNetCore.Mvc;
using WineClubApi.Data.Repositories;

namespace WineClubApi.Controllers.V1;

[ApiController]
[Route("api/v1/responsibilities")]
public sealed class ResponsibilitiesController(IResponsibilityRepository responsibilityRepository) : ControllerBase
{
    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming([FromQuery] long householdId, [FromQuery] int limit = 6, CancellationToken cancellationToken = default)
    {
        var result = await responsibilityRepository.GetUpcomingAsync(householdId, limit, cancellationToken);
        return Ok(result);
    }
}
