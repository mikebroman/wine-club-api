using WineClubApi.Api.V1.Dtos;

namespace WineClubApi.Data.Repositories;

public interface IResponsibilityRepository
{
    Task<IReadOnlyList<UpcomingResponsibilityItem>> GetUpcomingAsync(long householdId, int limit, CancellationToken cancellationToken);
}
