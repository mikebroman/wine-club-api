using WineClubApi.Api.V1.Dtos;

namespace WineClubApi.Data.Repositories;

public interface IAnnouncementRepository
{
    Task<CurrentAnnouncementResponse?> GetCurrentAsync(long userAccountId, IncludeOptions include, CancellationToken cancellationToken);

    Task<AnnouncementReactionStripResponse> SetMyReactionAsync(
        long userAccountId,
        long announcementId,
        string emoji,
        bool active,
        CancellationToken cancellationToken);
}
