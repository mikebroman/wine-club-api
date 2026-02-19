using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wine_club_api.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BottleTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BottleTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Provider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderSubject = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Theme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Households",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Households", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Households_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorUserAccountId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Announcements_UserAccounts_AuthorUserAccountId",
                        column: x => x.AuthorUserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventRsvps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<long>(type: "bigint", nullable: false),
                    UserAccountId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RespondedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRsvps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRsvps_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventRsvps_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bottles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<long>(type: "bigint", nullable: false),
                    Producer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VintageLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WineType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BroughtByHouseholdId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bottles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bottles_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bottles_Households_BroughtByHouseholdId",
                        column: x => x.BroughtByHouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventResponsibilities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<long>(type: "bigint", nullable: false),
                    ResponsibilityType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventResponsibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventResponsibilities_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventResponsibilities_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HouseholdMembers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseholdId = table.Column<long>(type: "bigint", nullable: false),
                    UserAccountId = table.Column<long>(type: "bigint", nullable: false),
                    MembershipRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoinedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseholdMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseholdMembers_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseholdMembers_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementReactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnnouncementId = table.Column<long>(type: "bigint", nullable: false),
                    UserAccountId = table.Column<long>(type: "bigint", nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementReactions_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnouncementReactions_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BottleNotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BottleId = table.Column<long>(type: "bigint", nullable: false),
                    UserAccountId = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BottleNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BottleNotes_Bottles_BottleId",
                        column: x => x.BottleId,
                        principalTable: "Bottles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BottleNotes_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BottleRatings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BottleId = table.Column<long>(type: "bigint", nullable: false),
                    UserAccountId = table.Column<long>(type: "bigint", nullable: false),
                    Rating = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BottleRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BottleRatings_Bottles_BottleId",
                        column: x => x.BottleId,
                        principalTable: "Bottles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BottleRatings_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BottleTagMaps",
                columns: table => new
                {
                    BottleId = table.Column<long>(type: "bigint", nullable: false),
                    BottleTagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BottleTagMaps", x => new { x.BottleId, x.BottleTagId });
                    table.ForeignKey(
                        name: "FK_BottleTagMaps_BottleTags_BottleTagId",
                        column: x => x.BottleTagId,
                        principalTable: "BottleTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BottleTagMaps_Bottles_BottleId",
                        column: x => x.BottleId,
                        principalTable: "Bottles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementReactions_AnnouncementId",
                table: "AnnouncementReactions",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementReactions_AnnouncementId_UserAccountId_Emoji",
                table: "AnnouncementReactions",
                columns: new[] { "AnnouncementId", "UserAccountId", "Emoji" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementReactions_UserAccountId",
                table: "AnnouncementReactions",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_AuthorUserAccountId",
                table: "Announcements",
                column: "AuthorUserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_ClubId",
                table: "Announcements",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_BottleNotes_BottleId",
                table: "BottleNotes",
                column: "BottleId");

            migrationBuilder.CreateIndex(
                name: "IX_BottleNotes_UserAccountId",
                table: "BottleNotes",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BottleRatings_BottleId",
                table: "BottleRatings",
                column: "BottleId");

            migrationBuilder.CreateIndex(
                name: "IX_BottleRatings_BottleId_UserAccountId",
                table: "BottleRatings",
                columns: new[] { "BottleId", "UserAccountId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BottleRatings_UserAccountId",
                table: "BottleRatings",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Bottles_BroughtByHouseholdId",
                table: "Bottles",
                column: "BroughtByHouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Bottles_EventId",
                table: "Bottles",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_BottleTagMaps_BottleTagId",
                table: "BottleTagMaps",
                column: "BottleTagId");

            migrationBuilder.CreateIndex(
                name: "IX_EventResponsibilities_EventId_ResponsibilityType",
                table: "EventResponsibilities",
                columns: new[] { "EventId", "ResponsibilityType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventResponsibilities_HouseholdId",
                table: "EventResponsibilities",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRsvps_EventId_UserAccountId",
                table: "EventRsvps",
                columns: new[] { "EventId", "UserAccountId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventRsvps_UserAccountId",
                table: "EventRsvps",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ClubId",
                table: "Events",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartsAtUtc",
                table: "Events",
                column: "StartsAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdMembers_HouseholdId",
                table: "HouseholdMembers",
                column: "HouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdMembers_UserAccountId",
                table: "HouseholdMembers",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Households_ClubId",
                table: "Households",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Provider_ProviderSubject",
                table: "UserAccounts",
                columns: new[] { "Provider", "ProviderSubject" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementReactions");

            migrationBuilder.DropTable(
                name: "BottleNotes");

            migrationBuilder.DropTable(
                name: "BottleRatings");

            migrationBuilder.DropTable(
                name: "BottleTagMaps");

            migrationBuilder.DropTable(
                name: "EventResponsibilities");

            migrationBuilder.DropTable(
                name: "EventRsvps");

            migrationBuilder.DropTable(
                name: "HouseholdMembers");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "BottleTags");

            migrationBuilder.DropTable(
                name: "Bottles");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Households");

            migrationBuilder.DropTable(
                name: "Clubs");
        }
    }
}
