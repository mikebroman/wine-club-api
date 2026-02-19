using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WineClubApi.Api;
using WineClubApi.Api.V1.Dtos;
using WineClubApi.Data.Repositories;

namespace WineClubApi.Controllers.V1;

[ApiController]
[Route("api/v1/events")]
[Authorize]
public sealed class EventsController(
    IUserContext userContext,
    IEventRepository eventRepository) : ControllerBase
{
    [HttpGet("next")]
    public async Task<IActionResult> GetNext([FromQuery] string? include, CancellationToken cancellationToken)
    {
        var result = await eventRepository.GetNextAsync(
            userContext.UserAccountId,
            new IncludeOptions(include ?? string.Empty),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{eventId:long}")]
    public async Task<IActionResult> GetById([FromRoute] long eventId, [FromQuery] string? include, CancellationToken cancellationToken)
    {
        var result = await eventRepository.GetByIdAsync(
            userContext.UserAccountId,
            eventId,
            new IncludeOptions(include ?? string.Empty),
            cancellationToken);

        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{eventId:long}/my-rsvp")]
    public async Task<IActionResult> PutMyRsvp([FromRoute] long eventId, [FromBody] UpdateMyRsvpRequest request, CancellationToken cancellationToken)
    {
        var result = await eventRepository.SetMyRsvpAsync(
            userContext.UserAccountId,
            eventId,
            request.Status,
            cancellationToken);

        return Ok(result);
    }
}
