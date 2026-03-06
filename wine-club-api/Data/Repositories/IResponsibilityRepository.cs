using WineClubApi.Api.V1.Dtos;

namespace WineClubApi.Data.Repositories;

public interface IResponsibilityRepository
{
    Task<IReadOnlyList<UpcomingResponsibilityItem>> GetUpcomingAsync(long userAccountId, long clubId, long householdId, int limit, CancellationToken cancellationToken);
}
