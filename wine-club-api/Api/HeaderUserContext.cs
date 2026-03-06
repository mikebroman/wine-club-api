namespace WineClubApi.Api;

public sealed class HeaderUserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
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

            if (!httpContext.Request.Headers.TryGetValue("X-UserAccountId", out var values) || values.Count == 0)
            {
                throw new InvalidOperationException("Missing X-UserAccountId header.");
            }

            if (!long.TryParse(values[0], out var userAccountId) || userAccountId <= 0)
            {
                throw new InvalidOperationException("Invalid X-UserAccountId header.");
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

            if (!httpContext.Request.Headers.TryGetValue("X-ClubId", out var values) || values.Count == 0)
            {
                throw new InvalidOperationException("Missing X-ClubId header.");
            }

            if (!long.TryParse(values[0], out var clubId) || clubId <= 0)
            {
                throw new InvalidOperationException("Invalid X-ClubId header.");
            }

            return clubId;
        }
    }
}
