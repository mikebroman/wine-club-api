using Microsoft.EntityFrameworkCore;
using WineClubApi.Api.V1.Dtos;

namespace WineClubApi.Data.Repositories;

public sealed class ResponsibilityRepository(WineClubDbContext db) : IResponsibilityRepository
{
    public async Task<IReadOnlyList<UpcomingResponsibilityItem>> GetUpcomingAsync(long householdId, int limit, CancellationToken cancellationToken)
    {
        if (limit <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(limit));
        }

        return await db.EventResponsibilities
            .Where(x => x.HouseholdId == householdId)
            .Join(
                db.Events,
                r => r.EventId,
                e => e.Id,
                (r, e) => new { r, e })
            .Where(x => x.e.StartsAtUtc >= DateTime.UtcNow)
            .OrderBy(x => x.e.StartsAtUtc)
            .Select(x => new UpcomingResponsibilityItem(x.e.Id, x.e.Title, x.e.StartsAtUtc, x.r.ResponsibilityType))
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}
