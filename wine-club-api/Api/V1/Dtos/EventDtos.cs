namespace WineClubApi.Api.V1.Dtos;

public sealed record EventAssignmentItem(string ResponsibilityType, long HouseholdId, string HouseholdName);

public sealed record MyRsvpItem(string Status, DateTime? RespondedUtc);

public sealed record EventResponse(
    long Id,
    string Title,
    DateTime StartsAtUtc,
    string? LocationName,
    string? Address,
    string? Theme,
    string? Notes,
    string Status,
    DateTime CreatedUtc,
    IReadOnlyList<EventAssignmentItem>? Assignments,
    MyRsvpItem? MyRsvp);

public sealed record UpdateMyRsvpRequest(string Status);

public sealed record UpdateMyRsvpResponse(long EventId, MyRsvpItem MyRsvp);

public sealed record UpcomingResponsibilityItem(long EventId, string EventTitle, DateTime EventStartsAtUtc, string ResponsibilityType);
