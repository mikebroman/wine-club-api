namespace WineClubApi.Data.Entities;

public sealed class EventRsvp
{
    public long Id { get; set; }

    public long EventId { get; set; }
    public Event Event { get; set; } = null!;

    public long UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;

    public string Status { get; set; } = string.Empty;
    public DateTime? RespondedUtc { get; set; }
}
