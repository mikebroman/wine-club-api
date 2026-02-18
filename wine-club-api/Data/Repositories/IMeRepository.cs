using WineClubApi.Api.V1.Dtos;

namespace WineClubApi.Data.Repositories;

public interface IMeRepository
{
    Task<MeResponse> GetMeAsync(long userAccountId, CancellationToken cancellationToken);
    Task<MeProfileResponse> GetProfileAsync(long userAccountId, CancellationToken cancellationToken);
}
