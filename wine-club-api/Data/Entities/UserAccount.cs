namespace WineClubApi.Data.Entities;

public sealed class UserAccount
{
    public long Id { get; set; }

    public string Provider { get; set; } = string.Empty;
    public string ProviderSubject { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
    public DateTime CreatedUtc { get; set; }

    public List<HouseholdMember> HouseholdMemberships { get; set; } = [];

    public List<EventRsvp> EventRsvps { get; set; } = [];

    public List<BottleRating> BottleRatings { get; set; } = [];
    public List<BottleNote> BottleNotes { get; set; } = [];

    public List<Announcement> AuthoredAnnouncements { get; set; } = [];
    public List<AnnouncementReaction> AnnouncementReactions { get; set; } = [];
}
