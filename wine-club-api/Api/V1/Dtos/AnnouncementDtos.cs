namespace WineClubApi.Api.V1.Dtos;

public sealed record AnnouncementReactionCountsItem(string Emoji, int Count);

public sealed record CurrentAnnouncementResponse(
    long Id,
    string Title,
    string Body,
    DateTime CreatedUtc,
    IReadOnlyList<string>? MyReactions,
    IReadOnlyList<AnnouncementReactionCountsItem>? ReactionCounts);

public sealed record UpsertAnnouncementReactionRequest(bool Active);

public sealed record AnnouncementReactionStripResponse(
    IReadOnlyList<string> MyReactions,
    IReadOnlyList<AnnouncementReactionCountsItem> ReactionCounts);
