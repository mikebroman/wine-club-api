namespace WineClubApi.Data.Entities;

public sealed class Club
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }

    public List<Household> Households { get; set; } = [];
    public List<Event> Events { get; set; } = [];
    public List<Announcement> Announcements { get; set; } = [];
}
