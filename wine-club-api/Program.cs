using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WineClubApi.Data;
using WineClubApi.Api;
using WineClubApi.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<WineClubDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, HeaderUserContext>();

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

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, "uploads")),
    RequestPath = "/uploads",
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
