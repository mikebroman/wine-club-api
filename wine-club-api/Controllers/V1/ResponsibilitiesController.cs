using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WineClubApi.Api;
using WineClubApi.Data.Repositories;

namespace WineClubApi.Controllers.V1;

[ApiController]
[Route("api/v1/responsibilities")]
[Authorize]
public sealed class ResponsibilitiesController(IUserContext userContext, IResponsibilityRepository responsibilityRepository) : ControllerBase
{
    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming([FromQuery] long householdId, [FromQuery] int limit = 6, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await responsibilityRepository.GetUpcomingAsync(userContext.UserAccountId, userContext.ClubId, householdId, limit, cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
