using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WineClubApi.Data;
using WineClubApi.Api;
using WineClubApi.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<WineClubDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, JwtUserContext>();

JwtBearerOptions ConfigureJwtBearer()
{
    var signingKey = builder.Configuration["JWT_SIGNING_KEY"];
    if (string.IsNullOrWhiteSpace(signingKey))
    {
        throw new InvalidOperationException("Missing JWT_SIGNING_KEY configuration.");
    }

    var issuer = builder.Configuration["JWT_ISSUER"] ?? "WineClubApi";
    var audience = builder.Configuration["JWT_AUDIENCE"] ?? "WineClubApi";

    return new JwtBearerOptions
    {
        MapInboundClaims = false,
        TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ClockSkew = TimeSpan.FromMinutes(1),
        },
        Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsJsonAsync(new { message = "Unauthorized" });
            },
        },
    };
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var configured = ConfigureJwtBearer();
        options.MapInboundClaims = configured.MapInboundClaims;
        options.TokenValidationParameters = configured.TokenValidationParameters;
        options.Events = configured.Events;
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        var origin = builder.Configuration["DEV_CORS_ORIGIN"] ?? "http://localhost:5173";
        policy
            .WithOrigins(origin)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddScoped<IMeRepository, MeRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IResponsibilityRepository, ResponsibilityRepository>();
builder.Services.AddScoped<IBottleRepository, BottleRepository>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, "uploads")),
//    RequestPath = "/uploads",
//});

app.UseHttpsRedirection();

app.UseCors("DevCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
