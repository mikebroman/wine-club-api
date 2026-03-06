using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WineClubApi.Api;

public sealed class JwtUserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public long UserAccountId
    {
        get
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                throw new InvalidOperationException("HTTP context is not available.");
            }

            var subject = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new InvalidOperationException("Missing sub claim.");
            }

            if (!long.TryParse(subject, out var userAccountId) || userAccountId <= 0)
            {
                throw new InvalidOperationException("Invalid sub claim.");
            }

            return userAccountId;
        }
    }

    public long ClubId
    {
        get
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                throw new InvalidOperationException("HTTP context is not available.");
            }

            var clubIdValue = httpContext.User.FindFirstValue("clubId");
            if (string.IsNullOrWhiteSpace(clubIdValue))
            {
                throw new InvalidOperationException("Missing clubId claim.");
            }

            if (!long.TryParse(clubIdValue, out var clubId) || clubId <= 0)
            {
                throw new InvalidOperationException("Invalid clubId claim.");
            }

            return clubId;
        }
    }
}
