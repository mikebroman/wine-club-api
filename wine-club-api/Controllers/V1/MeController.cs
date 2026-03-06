using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WineClubApi.Api;
using WineClubApi.Api.V1.Dtos;
using WineClubApi.Data.Repositories;

namespace WineClubApi.Controllers.V1;

[ApiController]
[Route("api/v1/me")]
[Authorize]
public sealed class MeController(IUserContext userContext, IMeRepository meRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var result = await meRepository.GetMeAsync(userContext.UserAccountId, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return Unauthorized(new { message = "Unauthorized" });
        }
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        try
        {
            MeProfileResponse result = await meRepository.GetProfileAsync(userContext.UserAccountId, userContext.ClubId, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return Unauthorized(new { message = "Unauthorized" });
        }
    }
}
