namespace WineClubApi.Data.Entities;

public sealed class HouseholdMember
{
    public long Id { get; set; }

    public long HouseholdId { get; set; }
    public Household Household { get; set; } = null!;

    public long UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;

    public string MembershipRole { get; set; } = string.Empty;
    public DateTime JoinedUtc { get; set; }
}
