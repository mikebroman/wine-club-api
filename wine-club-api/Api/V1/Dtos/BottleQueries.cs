namespace WineClubApi.Api.V1.Dtos;

public enum BottleSort
{
    Recent,
    TopRated,
}

public sealed record BottleListQuery(
    BottleSort Sort,
    int? Limit,
    string? Filter,
    string? Q,
    int? Page,
    int? PageSize);
