namespace WineClubApi.Data.Entities;

public sealed class BottleNote
{
    public long Id { get; set; }

    public long BottleId { get; set; }
    public Bottle Bottle { get; set; } = null!;

    public long UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;

    public string Note { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }
    public DateTime? UpdatedUtc { get; set; }
}
