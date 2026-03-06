namespace WineClubApi.Api;

public interface IUserContext
{
    long UserAccountId { get; }
    long ClubId { get; }
}
