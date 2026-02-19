namespace WineClubApi.Api.V1.Dtos;

public sealed record MeResponse(long UserAccountId, string Email, string DisplayName, string? PictureUrl);

public sealed record MeProfileResponse(string Email, string DisplayName, string? PictureUrl);
