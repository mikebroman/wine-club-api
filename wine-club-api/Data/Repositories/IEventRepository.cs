using WineClubApi.Api.V1.Dtos;

namespace WineClubApi.Data.Repositories;

public interface IEventRepository
{
    Task<EventResponse?> GetNextAsync(long userAccountId, IncludeOptions include, CancellationToken cancellationToken);
    Task<EventResponse?> GetByIdAsync(long userAccountId, long eventId, IncludeOptions include, CancellationToken cancellationToken);

    Task<UpdateMyRsvpResponse> SetMyRsvpAsync(long userAccountId, long eventId, string status, CancellationToken cancellationToken);
}
