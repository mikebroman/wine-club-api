namespace WineClubApi.Data.Entities;

public sealed class EventResponsibility
{
    public long Id { get; set; }

    public long EventId { get; set; }
    public Event Event { get; set; } = null!;

    public string ResponsibilityType { get; set; } = string.Empty;

    public long HouseholdId { get; set; }
    public Household Household { get; set; } = null!;
}
