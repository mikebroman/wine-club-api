namespace WineClubApi.Data.Entities;

public sealed class AnnouncementReaction
{
    public long Id { get; set; }

    public long AnnouncementId { get; set; }
    public Announcement Announcement { get; set; } = null!;

    public long UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;

    public string Emoji { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }
}
