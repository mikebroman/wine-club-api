namespace WineClubApi.Api.V1.Dtos;

public sealed record IncludeOptions(string Raw)
{
    public bool Has(string token) => Raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Contains(token, StringComparer.OrdinalIgnoreCase);
}
