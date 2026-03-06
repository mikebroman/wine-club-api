using Microsoft.EntityFrameworkCore;
using WineClubApi.Api.V1.Dtos;
using WineClubApi.Data.Entities;

namespace WineClubApi.Data.Repositories;

public sealed class BottleRepository(WineClubDbContext db) : IBottleRepository
{
    public async Task<IReadOnlyList<BottleListItem>> GetListAsync(long clubId, BottleListQuery query, CancellationToken cancellationToken)
    {
        IQueryable<Bottle> q = db.Bottles.Where(x => x.Event.ClubId == clubId);

        if (!string.IsNullOrWhiteSpace(query.Q))
        {
            var term = query.Q.Trim();
            q = q.Where(x => x.Producer.Contains(term) || x.Name.Contains(term));
        }

        if (query.Sort == BottleSort.Recent)
        {
            q = q.OrderByDescending(x => x.CreatedUtc);
        }
        else
        {
            q = q
                .Select(b => new
                {
                    Bottle = b,
                    Love = db.BottleRatings.Count(r => r.BottleId == b.Id && r.Rating == "love"),
                    Like = db.BottleRatings.Count(r => r.BottleId == b.Id && r.Rating == "like"),
                    Meh = db.BottleRatings.Count(r => r.BottleId == b.Id && r.Rating == "meh"),
                })
                .OrderByDescending(x => x.Love)
                .ThenByDescending(x => x.Like)
                .ThenBy(x => x.Meh)
                .ThenByDescending(x => x.Bottle.CreatedUtc)
                .Select(x => x.Bottle);
        }

        if (query.Limit is { } limit and > 0)
        {
            q = q.Take(limit);
        }

        if (query.Page is { } page && query.PageSize is { } pageSize && page > 0 && pageSize > 0)
        {
            q = q.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return await q.Select(x => new BottleListItem(
                x.Id,
                x.EventId,
                x.Producer,
                x.Name,
                x.VintageLabel,
                x.WineType,
                x.Region,
                x.ImageUrl))
            .ToListAsync(cancellationToken);
    }

    public async Task<BottleDetailsResponse?> GetByIdAsync(long userAccountId, long clubId, long bottleId, IncludeOptions include, int similarLimit, CancellationToken cancellationToken)
    {
        var bottle = await db.Bottles
            .Where(x => x.Id == bottleId && x.Event.ClubId == clubId)
            .Select(x => new
            {
                x.Id,
                x.EventId,
                x.Producer,
                x.Name,
                x.VintageLabel,
                x.WineType,
                x.Region,
                x.ImageUrl,
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (bottle is null)
        {
            return null;
        }

        BottleRatingSummary? summary = null;
        if (include.Has("ratingSummary"))
        {
            var grouped = await db.BottleRatings
                .Where(x => x.BottleId == bottleId)
                .GroupBy(x => x.Rating)
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            summary = new BottleRatingSummary(
                Love: grouped.SingleOrDefault(x => x.Rating == "love")?.Count ?? 0,
                Like: grouped.SingleOrDefault(x => x.Rating == "like")?.Count ?? 0,
                Meh: grouped.SingleOrDefault(x => x.Rating == "meh")?.Count ?? 0);
        }

        string? myRating = null;
        if (include.Has("myRating"))
        {
            myRating = await db.BottleRatings
                .Where(x => x.BottleId == bottleId && x.UserAccountId == userAccountId)
                .Select(x => x.Rating)
                .SingleOrDefaultAsync(cancellationToken);
        }

        BottleNoteItem? myNote = null;
        if (include.Has("myNote"))
        {
            myNote = await db.BottleNotes
                .Where(x => x.BottleId == bottleId && x.UserAccountId == userAccountId)
                .Select(x => new BottleNoteItem(x.Note, x.CreatedUtc, x.UpdatedUtc))
                .SingleOrDefaultAsync(cancellationToken);
        }

        IReadOnlyList<BottleListItem>? similar = null;
        if (include.Has("similar"))
        {
            var producer = bottle.Producer;
            similar = await db.Bottles
                .Where(x => x.Event.ClubId == clubId && x.Id != bottleId && x.Producer == producer)
                .OrderByDescending(x => x.CreatedUtc)
                .Take(similarLimit)
                .Select(x => new BottleListItem(x.Id, x.EventId, x.Producer, x.Name, x.VintageLabel, x.WineType, x.Region, x.ImageUrl))
                .ToListAsync(cancellationToken);
        }

        return new BottleDetailsResponse(
            bottle.Id,
            bottle.EventId,
            bottle.Producer,
            bottle.Name,
            bottle.VintageLabel,
            bottle.WineType,
            bottle.Region,
            bottle.ImageUrl,
            summary,
            myRating,
            myNote,
            similar);
    }

    public async Task<UpdateMyRatingResponse> SetMyRatingAsync(long userAccountId, long clubId, long bottleId, string? rating, CancellationToken cancellationToken)
    {
        var normalized = rating?.Trim().ToLowerInvariant();
        if (normalized is not (null or "love" or "like" or "meh"))
        {
            throw new ArgumentException("Invalid rating.", nameof(rating));
        }

        var bottleExists = await db.Bottles.AnyAsync(x => x.Id == bottleId && x.Event.ClubId == clubId, cancellationToken);
        if (!bottleExists)
        {
            throw new KeyNotFoundException($"Bottle {bottleId} not found.");
        }

        var existing = await db.BottleRatings
            .SingleOrDefaultAsync(x => x.BottleId == bottleId && x.UserAccountId == userAccountId, cancellationToken);

        if (normalized is null)
        {
            if (existing is not null)
            {
                db.BottleRatings.Remove(existing);
                await db.SaveChangesAsync(cancellationToken);
            }
        }
        else
        {
            if (existing is null)
            {
                existing = new BottleRating
                {
                    BottleId = bottleId,
                    UserAccountId = userAccountId,
                    CreatedUtc = DateTime.UtcNow,
                };
                db.BottleRatings.Add(existing);
            }

            existing.Rating = normalized;
            await db.SaveChangesAsync(cancellationToken);
        }

        var grouped = await db.BottleRatings
            .Where(x => x.BottleId == bottleId)
            .GroupBy(x => x.Rating)
            .Select(g => new { Rating = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var summary = new BottleRatingSummary(
            Love: grouped.SingleOrDefault(x => x.Rating == "love")?.Count ?? 0,
            Like: grouped.SingleOrDefault(x => x.Rating == "like")?.Count ?? 0,
            Meh: grouped.SingleOrDefault(x => x.Rating == "meh")?.Count ?? 0);

        return new UpdateMyRatingResponse(normalized, summary);
    }

    public async Task<UpdateMyNoteResponse> SetMyNoteAsync(long userAccountId, long clubId, long bottleId, string note, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(note))
        {
            throw new ArgumentException("Note is required.", nameof(note));
        }

        var bottleExists = await db.Bottles.AnyAsync(x => x.Id == bottleId && x.Event.ClubId == clubId, cancellationToken);
        if (!bottleExists)
        {
            throw new KeyNotFoundException($"Bottle {bottleId} not found.");
        }

        var existing = await db.BottleNotes
            .SingleOrDefaultAsync(x => x.BottleId == bottleId && x.UserAccountId == userAccountId, cancellationToken);

        if (existing is null)
        {
            existing = new BottleNote
            {
                BottleId = bottleId,
                UserAccountId = userAccountId,
                CreatedUtc = DateTime.UtcNow,
            };
            db.BottleNotes.Add(existing);
        }

        existing.Note = note;
        existing.UpdatedUtc = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);

        return new UpdateMyNoteResponse(new BottleNoteItem(existing.Note, existing.CreatedUtc, existing.UpdatedUtc));
    }

    public async Task<UploadBottlePhotoResponse> SavePhotoAsync(long clubId, long bottleId, IFormFile file, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(file);

        var bottleExists = await db.Bottles.AnyAsync(x => x.Id == bottleId && x.Event.ClubId == clubId, cancellationToken);
        if (!bottleExists)
        {
            throw new KeyNotFoundException($"Bottle {bottleId} not found.");
        }

        var ext = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(ext) || ext.Length > 8)
        {
            ext = ".jpg";
        }

        var uploadsDir = Path.Combine(AppContext.BaseDirectory, "uploads");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"bottle-{bottleId}-{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(uploadsDir, fileName);

        await using (var stream = File.Create(fullPath))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        var imageUrl = $"/uploads/{fileName}";

        var bottle = await db.Bottles.SingleOrDefaultAsync(x => x.Id == bottleId, cancellationToken);
        if (bottle is null)
        {
            throw new InvalidOperationException($"Bottle {bottleId} not found.");
        }

        bottle.ImageUrl = imageUrl;
        await db.SaveChangesAsync(cancellationToken);

        return new UploadBottlePhotoResponse(imageUrl);
    }
}
