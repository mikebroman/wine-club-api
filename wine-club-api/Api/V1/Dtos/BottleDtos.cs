namespace WineClubApi.Api.V1.Dtos;

public sealed record BottleListItem(
    long Id,
    long EventId,
    string Producer,
    string Name,
    string? VintageLabel,
    string WineType,
    string? Region,
    string? ImageUrl);

public sealed record BottleRatingSummary(int Love, int Like, int Meh);

public sealed record BottleNoteItem(string Note, DateTime CreatedUtc, DateTime? UpdatedUtc);

public sealed record BottleDetailsResponse(
    long Id,
    long EventId,
    string Producer,
    string Name,
    string? VintageLabel,
    string WineType,
    string? Region,
    string? ImageUrl,
    BottleRatingSummary? RatingSummary,
    string? MyRating,
    BottleNoteItem? MyNote,
    IReadOnlyList<BottleListItem>? Similar);

public sealed record UpdateMyRatingRequest(string? Rating);

public sealed record UpdateMyRatingResponse(string? MyRating, BottleRatingSummary RatingSummary);

public sealed record UpdateMyNoteRequest(string Note);

public sealed record UpdateMyNoteResponse(BottleNoteItem MyNote);

public sealed record UploadBottlePhotoResponse(string ImageUrl);
