namespace WineClubApi.Data.Entities;

public sealed class BottleTag
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<BottleTagMap> BottleMaps { get; set; } = [];
}
