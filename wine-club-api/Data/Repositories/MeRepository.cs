using Microsoft.EntityFrameworkCore;
using WineClubApi.Api.V1.Dtos;

namespace WineClubApi.Data.Repositories;

public sealed class MeRepository(WineClubDbContext db) : IMeRepository
{
    public async Task<MeResponse> GetMeAsync(long userAccountId, CancellationToken cancellationToken)
    {
        var me = await db.UserAccounts
            .Where(x => x.Id == userAccountId)
            .Select(x => new MeResponse(x.Id, x.Email, x.DisplayName, x.PictureUrl))
            .SingleOrDefaultAsync(cancellationToken);

        return me ?? throw new InvalidOperationException($"UserAccount {userAccountId} not found.");
    }

    public async Task<MeProfileResponse> GetProfileAsync(long userAccountId, long clubId, CancellationToken cancellationToken)
    {
        var me = await db.UserAccounts
            .Where(x => x.Id == userAccountId)
            .Select(x => new MeProfileResponse(
                x.Email,
                x.DisplayName,
                x.PictureUrl,
                clubId,
                x.HouseholdMemberships
                    .Where(m => m.Household.IsActive)
                    .Select(m => new
                    {
                        ClubId = m.Household.ClubId,
                        ClubName = m.Household.Club.Name,
                        ClubTimeZone = m.Household.Club.TimeZone,
                    })
                    .Distinct()
                    .OrderBy(c => c.ClubName)
                    .Select(c => new MyClubResponse(c.ClubId, c.ClubName, c.ClubTimeZone))
                    .ToList(),
                x.HouseholdMemberships
                    .Where(m => m.Household.ClubId == clubId)
                    .OrderBy(m => m.Household.Name)
                    .Select(m => new HouseholdMembershipResponse(
                        m.Household.ClubId,
                        m.Household.Club.Name,
                        m.HouseholdId,
                        m.Household.Name,
                        m.Household.IsActive,
                        m.MembershipRole,
                        m.JoinedUtc))
                    .ToList()))
            .SingleOrDefaultAsync(cancellationToken);

        return me ?? throw new InvalidOperationException($"UserAccount {userAccountId} not found.");
    }
}
