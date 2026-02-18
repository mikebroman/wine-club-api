namespace WineClubApi.Data.Entities;

public sealed class Event
{
    public long Id { get; set; }

    public long ClubId { get; set; }
    public Club Club { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public DateTime StartsAtUtc { get; set; }

    public string? LocationName { get; set; }
    public string? Address { get; set; }
    public string? Theme { get; set; }
    public string? Notes { get; set; }

    public string Status { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }

    public List<EventResponsibility> Responsibilities { get; set; } = [];
    public List<EventRsvp> Rsvps { get; set; } = [];
    public List<Bottle> Bottles { get; set; } = [];
}
