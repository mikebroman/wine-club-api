using Microsoft.AspNetCore.Mvc;
using WineClubApi.Api;
using WineClubApi.Api.V1.Dtos;
using WineClubApi.Data;
using WineClubApi.Data.Repositories;

namespace WineClubApi.Controllers.V1;

[ApiController]
[Route("api/v1/announcements")]
public sealed class AnnouncementsController(
    IUserContext userContext,
    WineClubDbContext db,
    IAnnouncementRepository announcementRepository) : ControllerBase
{
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent([FromQuery] string? include, CancellationToken cancellationToken)
    {
        await UserAccountSeeder.EnsureUserAccountExistsAsync(db, userContext.UserAccountId, cancellationToken);

        var result = await announcementRepository.GetCurrentAsync(
            userContext.UserAccountId,
            new IncludeOptions(include ?? string.Empty),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{announcementId:long}/my-reactions/{emoji}")]
    public async Task<IActionResult> PutMyReaction(
        [FromRoute] long announcementId,
        [FromRoute] string emoji,
        [FromBody] UpsertAnnouncementReactionRequest request,
        CancellationToken cancellationToken)
    {
        await UserAccountSeeder.EnsureUserAccountExistsAsync(db, userContext.UserAccountId, cancellationToken);

        var result = await announcementRepository.SetMyReactionAsync(
            userContext.UserAccountId,
            announcementId,
            emoji,
            request.Active,
            cancellationToken);

        return Ok(result);
    }
}
