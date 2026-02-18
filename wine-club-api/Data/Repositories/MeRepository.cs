using Microsoft.EntityFrameworkCore;
using WineClubApi.Api.V1.Dtos;

namespace WineClubApi.Data.Repositories;

public sealed class MeRepository(WineClubDbContext db) : IMeRepository
{
    public async Task<MeResponse> GetMeAsync(long userAccountId, CancellationToken cancellationToken)
    {
        var me = await db.UserAccounts
            .Where(x => x.Id == userAccountId)
            .Select(x => new MeResponse(x.Id, x.Email, x.DisplayName))
            .SingleOrDefaultAsync(cancellationToken);

        return me ?? throw new InvalidOperationException($"UserAccount {userAccountId} not found.");
    }

    public async Task<MeProfileResponse> GetProfileAsync(long userAccountId, CancellationToken cancellationToken)
    {
        var me = await db.UserAccounts
            .Where(x => x.Id == userAccountId)
            .Select(x => new MeProfileResponse(x.Email, x.DisplayName))
            .SingleOrDefaultAsync(cancellationToken);

        return me ?? throw new InvalidOperationException($"UserAccount {userAccountId} not found.");
    }
}
