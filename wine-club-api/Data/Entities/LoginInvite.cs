namespace WineClubApi.Data.Entities;

public sealed class LoginInvite
{
    public long Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public long ClubId { get; set; }

    public DateTime CreatedUtc { get; set; }

    public Club Club { get; set; } = null!;
}
