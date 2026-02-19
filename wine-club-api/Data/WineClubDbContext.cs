using Microsoft.EntityFrameworkCore;
using WineClubApi.Data.Entities;

namespace WineClubApi.Data;

public sealed class WineClubDbContext(DbContextOptions<WineClubDbContext> options) : DbContext(options)
{
    public DbSet<Club> Clubs => Set<Club>();
    public DbSet<Household> Households => Set<Household>();
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    public DbSet<HouseholdMember> HouseholdMembers => Set<HouseholdMember>();

    public DbSet<LoginInvite> LoginInvites => Set<LoginInvite>();

    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventResponsibility> EventResponsibilities => Set<EventResponsibility>();
    public DbSet<EventRsvp> EventRsvps => Set<EventRsvp>();

    public DbSet<Bottle> Bottles => Set<Bottle>();
    public DbSet<BottleTag> BottleTags => Set<BottleTag>();
    public DbSet<BottleTagMap> BottleTagMaps => Set<BottleTagMap>();
    public DbSet<BottleRating> BottleRatings => Set<BottleRating>();
    public DbSet<BottleNote> BottleNotes => Set<BottleNote>();

    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<AnnouncementReaction> AnnouncementReactions => Set<AnnouncementReaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LoginInvite>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<UserAccount>()
            .HasIndex(x => new { x.Provider, x.ProviderSubject })
            .IsUnique();

        modelBuilder.Entity<Event>()
            .HasIndex(x => x.StartsAtUtc);

        modelBuilder.Entity<EventRsvp>()
            .HasIndex(x => new { x.EventId, x.UserAccountId })
            .IsUnique();

        modelBuilder.Entity<EventResponsibility>()
            .HasIndex(x => new { x.EventId, x.ResponsibilityType })
            .IsUnique();

        modelBuilder.Entity<EventResponsibility>()
            .HasOne(x => x.Household)
            .WithMany(x => x.EventResponsibilities)
            .HasForeignKey(x => x.HouseholdId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Bottle>()
            .HasIndex(x => x.EventId);

        modelBuilder.Entity<BottleRating>()
            .HasIndex(x => x.BottleId);

        modelBuilder.Entity<BottleRating>()
            .HasIndex(x => new { x.BottleId, x.UserAccountId })
            .IsUnique();

        modelBuilder.Entity<AnnouncementReaction>()
            .HasIndex(x => x.AnnouncementId);

        modelBuilder.Entity<AnnouncementReaction>()
            .HasIndex(x => new { x.AnnouncementId, x.UserAccountId, x.Emoji })
            .IsUnique();

        modelBuilder.Entity<BottleTagMap>()
            .HasKey(x => new { x.BottleId, x.BottleTagId });

        modelBuilder.Entity<BottleTagMap>()
            .HasOne(x => x.Bottle)
            .WithMany(x => x.TagMaps)
            .HasForeignKey(x => x.BottleId);

        modelBuilder.Entity<BottleTagMap>()
            .HasOne(x => x.BottleTag)
            .WithMany(x => x.BottleMaps)
            .HasForeignKey(x => x.BottleTagId);

        modelBuilder.Entity<Bottle>()
            .HasOne(x => x.BroughtByHousehold)
            .WithMany(x => x.BottlesBrought)
            .HasForeignKey(x => x.BroughtByHouseholdId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Announcement>()
            .HasOne(x => x.AuthorUserAccount)
            .WithMany(x => x.AuthoredAnnouncements)
            .HasForeignKey(x => x.AuthorUserAccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
