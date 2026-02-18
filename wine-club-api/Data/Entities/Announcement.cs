namespace WineClubApi.Data.Entities;

public sealed class Announcement
{
    public long Id { get; set; }

    public long ClubId { get; set; }
    public Club Club { get; set; } = null!;

    public long AuthorUserAccountId { get; set; }
    public UserAccount AuthorUserAccount { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; }

    public List<AnnouncementReaction> Reactions { get; set; } = [];
}
