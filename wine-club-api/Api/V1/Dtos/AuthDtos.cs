namespace WineClubApi.Api.V1.Dtos;

public sealed record GoogleAuthRequest(string Credential);

public sealed record AuthResponse(string AccessToken, MeResponse User);
