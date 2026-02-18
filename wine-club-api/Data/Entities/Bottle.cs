namespace WineClubApi.Data.Entities;

public sealed class Bottle
{
    public long Id { get; set; }

    public long EventId { get; set; }
    public Event Event { get; set; } = null!;

    public string Producer { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? VintageLabel { get; set; }

    public string WineType { get; set; } = string.Empty;

    public string? Region { get; set; }
    public string? ImageUrl { get; set; }

    public long? BroughtByHouseholdId { get; set; }
    public Household? BroughtByHousehold { get; set; }

    public DateTime CreatedUtc { get; set; }

    public List<BottleTagMap> TagMaps { get; set; } = [];
    public List<BottleRating> Ratings { get; set; } = [];
    public List<BottleNote> Notes { get; set; } = [];
}
