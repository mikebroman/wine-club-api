using Microsoft.EntityFrameworkCore;
using WineClubApi.Data;
using WineClubApi.Data.Entities;

namespace WineClubApi.Api;

public static class UserAccountSeeder
{
    public static async Task EnsureUserAccountExistsAsync(
        WineClubDbContext db,
        long userAccountId,
        CancellationToken cancellationToken)
    {
        var exists = await db.UserAccounts.AnyAsync(x => x.Id == userAccountId, cancellationToken);
        if (exists)
        {
            return;
        }

        db.UserAccounts.Add(new UserAccount
        {
            Id = userAccountId,
            Provider = "Google",
            ProviderSubject = $"seed:{userAccountId}",
            Email = $"user{userAccountId}@example.com",
            DisplayName = $"User {userAccountId}",
            IsActive = true,
            CreatedUtc = DateTime.UtcNow,
        });

        await db.SaveChangesAsync(cancellationToken);
    }
}
