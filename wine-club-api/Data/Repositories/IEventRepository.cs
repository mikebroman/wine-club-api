using WineClubApi.Api.V1.Dtos;

namespace WineClubApi.Data.Repositories;

public interface IEventRepository
{
    Task<EventResponse?> GetNextAsync(long userAccountId, long clubId, IncludeOptions include, CancellationToken cancellationToken);
    Task<EventResponse?> GetByIdAsync(long userAccountId, long clubId, long eventId, IncludeOptions include, CancellationToken cancellationToken);

    Task<UpdateMyRsvpResponse> SetMyRsvpAsync(long userAccountId, long clubId, long eventId, string status, CancellationToken cancellationToken);
}
