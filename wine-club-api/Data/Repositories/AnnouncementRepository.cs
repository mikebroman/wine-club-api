using Microsoft.EntityFrameworkCore;
using WineClubApi.Api.V1.Dtos;
using WineClubApi.Data.Entities;

namespace WineClubApi.Data.Repositories;

public sealed class AnnouncementRepository(WineClubDbContext db) : IAnnouncementRepository
{
    public async Task<CurrentAnnouncementResponse?> GetCurrentAsync(long userAccountId, IncludeOptions include, CancellationToken cancellationToken)
    {
        var announcement = await db.Announcements
            .OrderByDescending(x => x.CreatedUtc)
            .Select(x => new
            {
                x.Id,
                x.Title,
                x.Body,
                x.CreatedUtc,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (announcement is null)
        {
            return null;
        }

        IReadOnlyList<string>? myReactions = null;
        if (include.Has("myReactions"))
        {
            myReactions = await db.AnnouncementReactions
                .Where(x => x.AnnouncementId == announcement.Id && x.UserAccountId == userAccountId)
                .OrderBy(x => x.Emoji)
                .Select(x => x.Emoji)
                .ToListAsync(cancellationToken);
        }

        IReadOnlyList<AnnouncementReactionCountsItem>? reactionCounts = null;
        if (include.Has("reactionCounts"))
        {
            reactionCounts = await db.AnnouncementReactions
                .Where(x => x.AnnouncementId == announcement.Id)
                .GroupBy(x => x.Emoji)
                .Select(g => new AnnouncementReactionCountsItem(g.Key, g.Count()))
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Emoji)
                .ToListAsync(cancellationToken);
        }

        return new CurrentAnnouncementResponse(
            announcement.Id,
            announcement.Title,
            announcement.Body,
            announcement.CreatedUtc,
            myReactions,
            reactionCounts);
    }

    public async Task<AnnouncementReactionStripResponse> SetMyReactionAsync(
        long userAccountId,
        long announcementId,
        string emoji,
        bool active,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(emoji))
        {
            throw new ArgumentException("Emoji is required.", nameof(emoji));
        }

        var existing = await db.AnnouncementReactions
            .SingleOrDefaultAsync(
                x => x.AnnouncementId == announcementId && x.UserAccountId == userAccountId && x.Emoji == emoji,
                cancellationToken);

        if (active)
        {
            if (existing is null)
            {
                db.AnnouncementReactions.Add(new AnnouncementReaction
                {
                    AnnouncementId = announcementId,
                    UserAccountId = userAccountId,
                    Emoji = emoji,
                    CreatedUtc = DateTime.UtcNow,
                });

                await db.SaveChangesAsync(cancellationToken);
            }
        }
        else
        {
            if (existing is not null)
            {
                db.AnnouncementReactions.Remove(existing);
                await db.SaveChangesAsync(cancellationToken);
            }
        }

        var myReactions = await db.AnnouncementReactions
            .Where(x => x.AnnouncementId == announcementId && x.UserAccountId == userAccountId)
            .OrderBy(x => x.Emoji)
            .Select(x => x.Emoji)
            .ToListAsync(cancellationToken);

        var reactionCounts = await db.AnnouncementReactions
            .Where(x => x.AnnouncementId == announcementId)
            .GroupBy(x => x.Emoji)
            .Select(g => new AnnouncementReactionCountsItem(g.Key, g.Count()))
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Emoji)
            .ToListAsync(cancellationToken);

        return new AnnouncementReactionStripResponse(myReactions, reactionCounts);
    }
}
