namespace WineClubApi.Data.Entities;

public sealed class BottleTagMap
{
    public long BottleId { get; set; }
    public Bottle Bottle { get; set; } = null!;

    public long BottleTagId { get; set; }
    public BottleTag BottleTag { get; set; } = null!;
}
