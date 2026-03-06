using Microsoft.EntityFrameworkCore;
using WineClubApi.Api.V1.Dtos;

namespace WineClubApi.Data.Repositories;

public sealed class ResponsibilityRepository(WineClubDbContext db) : IResponsibilityRepository
{
    public async Task<IReadOnlyList<UpcomingResponsibilityItem>> GetUpcomingAsync(long userAccountId, long clubId, long householdId, int limit, CancellationToken cancellationToken)
    {
        if (limit <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(limit));
        }

        var hasMembership = await db.HouseholdMembers
            .AnyAsync(
                x => x.UserAccountId == userAccountId && x.HouseholdId == householdId && x.Household.ClubId == clubId,
                cancellationToken);

        if (!hasMembership)
        {
            throw new UnauthorizedAccessException("Not authorized for this household.");
        }

        return await db.EventResponsibilities
            .Where(x => x.HouseholdId == householdId)
            .Join(
                db.Events,
                r => r.EventId,
                e => e.Id,
                (r, e) => new { r, e })
            .Where(x => x.e.ClubId == clubId)
            .Where(x => x.e.StartsAtUtc >= DateTime.UtcNow)
            .OrderBy(x => x.e.StartsAtUtc)
            .Select(x => new UpcomingResponsibilityItem(x.e.Id, x.e.Title, x.e.StartsAtUtc, x.r.ResponsibilityType))
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}
