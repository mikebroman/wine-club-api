using Microsoft.AspNetCore.Mvc;
using WineClubApi.Api;
using WineClubApi.Data;
using WineClubApi.Data.Repositories;

namespace WineClubApi.Controllers.V1;

[ApiController]
[Route("api/v1/me")]
public sealed class MeController(IUserContext userContext, WineClubDbContext db, IMeRepository meRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        await UserAccountSeeder.EnsureUserAccountExistsAsync(db, userContext.UserAccountId, cancellationToken);
        var result = await meRepository.GetMeAsync(userContext.UserAccountId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        await UserAccountSeeder.EnsureUserAccountExistsAsync(db, userContext.UserAccountId, cancellationToken);
        var result = await meRepository.GetProfileAsync(userContext.UserAccountId, cancellationToken);
        return Ok(result);
    }
}
