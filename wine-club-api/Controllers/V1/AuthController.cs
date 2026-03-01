using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WineClubApi.Api.V1.Dtos;
using WineClubApi.Data;
using WineClubApi.Data.Entities;

namespace WineClubApi.Controllers.V1;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController(IConfiguration configuration, WineClubDbContext db) : ControllerBase
{
    [HttpGet("unauthorized")]
    public IActionResult UnauthorizedTest()
    {
        return Unauthorized(new { message = "Unauthorized" });
    }

    [HttpPost("google")]
    public async Task<IActionResult> Google([FromBody] GoogleAuthRequest? request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request?.Credential))
        {
            return BadRequest(new { message = "Missing credential" });
        }

        var googleClientId = configuration["GOOGLE_CLIENT_ID"];
        if (string.IsNullOrWhiteSpace(googleClientId))
        {
            throw new InvalidOperationException("Missing GOOGLE_CLIENT_ID configuration.");
        }

        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(request.Credential, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [googleClientId],
            });
        }
        catch (Exception)
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        if (string.IsNullOrWhiteSpace(payload.Subject) || string.IsNullOrWhiteSpace(payload.Email))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        if (payload.EmailVerified != true)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Email not verified" });
        }

        var email = payload.Email;

        var invitation = await db.LoginInvites.FirstOrDefaultAsync(x => x.Email == email && x.IsActive, cancellationToken);
        
        if (invitation == null)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Not authorized" });
        }

        var provider = "Google";
        var providerSubject = payload.Subject;

        var user = await db.UserAccounts
            .SingleOrDefaultAsync(x => x.Provider == provider && x.ProviderSubject == providerSubject, cancellationToken);

        if (user is null)
        {
            user = await db.UserAccounts
                .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);

            if (user is null)
            {
                user = new UserAccount
                {
                    Provider = provider,
                    ProviderSubject = providerSubject,
                    Email = email,
                    DisplayName = payload.Name ?? string.Empty,
                    PictureUrl = payload.Picture,
                    IsActive = true,
                    ClubId = invitation.ClubId,
                    CreatedUtc = DateTime.UtcNow,
                };

                db.UserAccounts.Add(user);
            }
            else
            {
                user.Provider = provider;
                user.ProviderSubject = providerSubject;
            }
        }

        user.Email = email;
        user.DisplayName = payload.Name ?? user.DisplayName;
        user.PictureUrl = payload.Picture;
        user.IsActive = true;

        await db.SaveChangesAsync(cancellationToken);

        var accessToken = CreateAccessToken(user);
        var me = new MeResponse(user.Id, user.Email, user.DisplayName, user.PictureUrl);

        return Ok(new AuthResponse(accessToken, me));
    }

    private string CreateAccessToken(UserAccount user)
    {
        var signingKey = configuration["JWT_SIGNING_KEY"];
        if (string.IsNullOrWhiteSpace(signingKey))
        {
            throw new InvalidOperationException("Missing JWT_SIGNING_KEY configuration.");
        }

        var issuer = configuration["JWT_ISSUER"] ?? "WineClubApi";
        var audience = configuration["JWT_AUDIENCE"] ?? "WineClubApi";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
