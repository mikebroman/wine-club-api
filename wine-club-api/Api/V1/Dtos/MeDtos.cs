namespace WineClubApi.Api.V1.Dtos;

public sealed record MeResponse(long UserAccountId, string Email, string DisplayName, string? PictureUrl);

public sealed record MyClubResponse(long ClubId, string ClubName, string ClubTimeZone);

public sealed record HouseholdMembershipResponse(
    long ClubId,
    string ClubName,
    long HouseholdId,
    string HouseholdName,
    bool HouseholdIsActive,
    string MembershipRole,
    DateTime JoinedUtc);

public sealed record MeProfileResponse(
    string Email,
    string DisplayName,
    string? PictureUrl,
    long ActiveClubId,
    IReadOnlyList<MyClubResponse> Clubs,
    IReadOnlyList<HouseholdMembershipResponse> HouseholdMemberships);
