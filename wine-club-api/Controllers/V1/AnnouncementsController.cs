using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WineClubApi.Api;
using WineClubApi.Api.V1.Dtos;
using WineClubApi.Data.Repositories;

namespace WineClubApi.Controllers.V1;

[ApiController]
[Route("api/v1/announcements")]
[Authorize]
public sealed class AnnouncementsController(
    IUserContext userContext,
    IAnnouncementRepository announcementRepository) : ControllerBase
{
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent([FromQuery] string? include, CancellationToken cancellationToken)
    {
        var result = await announcementRepository.GetCurrentAsync(
            userContext.UserAccountId,
            userContext.ClubId,
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
        try
        {
            var result = await announcementRepository.SetMyReactionAsync(
                userContext.UserAccountId,
                userContext.ClubId,
                announcementId,
                emoji,
                request.Active,
                cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
