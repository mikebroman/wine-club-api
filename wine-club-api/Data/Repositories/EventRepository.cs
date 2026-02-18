using Microsoft.EntityFrameworkCore;
using WineClubApi.Api.V1.Dtos;
using WineClubApi.Data.Entities;

namespace WineClubApi.Data.Repositories;

public sealed class EventRepository(WineClubDbContext db) : IEventRepository
{
    public Task<EventResponse?> GetNextAsync(long userAccountId, IncludeOptions include, CancellationToken cancellationToken) =>
        GetSingleAsync(
            userAccountId,
            include,
            q => q.OrderBy(x => x.StartsAtUtc).Where(x => x.StartsAtUtc >= DateTime.UtcNow),
            cancellationToken);

    public Task<EventResponse?> GetByIdAsync(long userAccountId, long eventId, IncludeOptions include, CancellationToken cancellationToken) =>
        GetSingleAsync(
            userAccountId,
            include,
            q => q.Where(x => x.Id == eventId),
            cancellationToken);

    public async Task<UpdateMyRsvpResponse> SetMyRsvpAsync(long userAccountId, long eventId, string status, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            throw new ArgumentException("Status is required.", nameof(status));
        }

        var normalized = status.Trim().ToLowerInvariant();
        if (normalized is not ("none" or "accepted" or "tentative" or "declined"))
        {
            throw new ArgumentException("Invalid RSVP status.", nameof(status));
        }

        var existing = await db.EventRsvps
            .SingleOrDefaultAsync(x => x.EventId == eventId && x.UserAccountId == userAccountId, cancellationToken);

        if (normalized == "none")
        {
            if (existing is not null)
            {
                db.EventRsvps.Remove(existing);
                await db.SaveChangesAsync(cancellationToken);
            }

            return new UpdateMyRsvpResponse(eventId, new MyRsvpItem("none", null));
        }

        if (existing is null)
        {
            existing = new EventRsvp
            {
                EventId = eventId,
                UserAccountId = userAccountId,
            };
            db.EventRsvps.Add(existing);
        }

        existing.Status = normalized;
        existing.RespondedUtc = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);

        return new UpdateMyRsvpResponse(eventId, new MyRsvpItem(existing.Status, existing.RespondedUtc));
    }

    private async Task<EventResponse?> GetSingleAsync(
        long userAccountId,
        IncludeOptions include,
        Func<IQueryable<Event>, IQueryable<Event>> apply,
        CancellationToken cancellationToken)
    {
        var evt = await apply(db.Events).Select(x => new
            {
                x.Id,
                x.Title,
                x.StartsAtUtc,
                x.LocationName,
                x.Address,
                x.Theme,
                x.Notes,
                x.Status,
                x.CreatedUtc,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (evt is null)
        {
            return null;
        }

        IReadOnlyList<EventAssignmentItem>? assignments = null;
        if (include.Has("assignments"))
        {
            assignments = await db.EventResponsibilities
                .Where(x => x.EventId == evt.Id)
                .Join(db.Households, er => er.HouseholdId, h => h.Id,
                    (er, h) => new EventAssignmentItem(er.ResponsibilityType, h.Id, h.Name))
                .OrderBy(x => x.ResponsibilityType)
                .ToListAsync(cancellationToken);
        }

        MyRsvpItem? myRsvp = null;
        if (include.Has("myRsvp"))
        {
            myRsvp = await db.EventRsvps
                .Where(x => x.EventId == evt.Id && x.UserAccountId == userAccountId)
                .Select(x => new MyRsvpItem(x.Status, x.RespondedUtc))
                .SingleOrDefaultAsync(cancellationToken);

            myRsvp ??= new MyRsvpItem("none", null);
        }

        return new EventResponse(
            evt.Id,
            evt.Title,
            evt.StartsAtUtc,
            evt.LocationName,
            evt.Address,
            evt.Theme,
            evt.Notes,
            evt.Status,
            evt.CreatedUtc,
            assignments,
            myRsvp);
    }
}
