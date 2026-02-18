using WineClubApi.Api.V1.Dtos;

namespace WineClubApi.Data.Repositories;

public interface IBottleRepository
{
    Task<IReadOnlyList<BottleListItem>> GetListAsync(BottleListQuery query, CancellationToken cancellationToken);

    Task<BottleDetailsResponse?> GetByIdAsync(long userAccountId, long bottleId, IncludeOptions include, int similarLimit, CancellationToken cancellationToken);

    Task<UpdateMyRatingResponse> SetMyRatingAsync(long userAccountId, long bottleId, string? rating, CancellationToken cancellationToken);
    Task<UpdateMyNoteResponse> SetMyNoteAsync(long userAccountId, long bottleId, string note, CancellationToken cancellationToken);

    Task<UploadBottlePhotoResponse> SavePhotoAsync(long bottleId, IFormFile file, CancellationToken cancellationToken);
}
