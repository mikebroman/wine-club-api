using Microsoft.AspNetCore.Mvc;
using WineClubApi.Api;
using WineClubApi.Api.V1.Dtos;
using WineClubApi.Data;
using WineClubApi.Data.Repositories;

namespace WineClubApi.Controllers.V1;

[ApiController]
[Route("api/v1/bottles")]
public sealed class BottlesController(
    IUserContext userContext,
    WineClubDbContext db,
    IBottleRepository bottleRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] string? sort,
        [FromQuery] int? limit,
        [FromQuery] string? filter,
        [FromQuery] string? q,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        var parsedSort = string.Equals(sort, "topRated", StringComparison.OrdinalIgnoreCase)
            ? BottleSort.TopRated
            : BottleSort.Recent;

        var result = await bottleRepository.GetListAsync(
            new BottleListQuery(parsedSort, limit, filter, q, page, pageSize),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{bottleId:long}")]
    public async Task<IActionResult> GetById([FromRoute] long bottleId, [FromQuery] string? include, CancellationToken cancellationToken)
    {
        await UserAccountSeeder.EnsureUserAccountExistsAsync(db, userContext.UserAccountId, cancellationToken);

        var includeOptions = new IncludeOptions(include ?? string.Empty);
        var similarLimit = 3;

        var result = await bottleRepository.GetByIdAsync(
            userContext.UserAccountId,
            bottleId,
            includeOptions,
            similarLimit,
            cancellationToken);

        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{bottleId:long}/my-rating")]
    public async Task<IActionResult> PutMyRating(
        [FromRoute] long bottleId,
        [FromBody] UpdateMyRatingRequest request,
        CancellationToken cancellationToken)
    {
        await UserAccountSeeder.EnsureUserAccountExistsAsync(db, userContext.UserAccountId, cancellationToken);

        var result = await bottleRepository.SetMyRatingAsync(userContext.UserAccountId, bottleId, request.Rating, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{bottleId:long}/my-note")]
    public async Task<IActionResult> PutMyNote(
        [FromRoute] long bottleId,
        [FromBody] UpdateMyNoteRequest request,
        CancellationToken cancellationToken)
    {
        await UserAccountSeeder.EnsureUserAccountExistsAsync(db, userContext.UserAccountId, cancellationToken);

        var result = await bottleRepository.SetMyNoteAsync(userContext.UserAccountId, bottleId, request.Note, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{bottleId:long}/photo")]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> UploadPhoto(
        [FromRoute] long bottleId,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        await UserAccountSeeder.EnsureUserAccountExistsAsync(db, userContext.UserAccountId, cancellationToken);

        var result = await bottleRepository.SavePhotoAsync(bottleId, file, cancellationToken);
        return Ok(result);
    }
}
