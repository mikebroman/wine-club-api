namespace WineClubApi.Data.Entities;

public sealed class Household
{
    public long Id { get; set; }

    public long ClubId { get; set; }
    public Club Club { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedUtc { get; set; }

    public List<HouseholdMember> Members { get; set; } = [];

    public List<EventResponsibility> EventResponsibilities { get; set; } = [];

    public List<Bottle> BottlesBrought { get; set; } = [];
}
