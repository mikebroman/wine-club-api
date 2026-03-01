using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wine_club_api.Migrations
{
    /// <inheritdoc />
    public partial class UserClubs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ClubId",
                table: "UserAccounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ClubId",
                table: "LoginInvites",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_ClubId",
                table: "UserAccounts",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginInvites_ClubId",
                table: "LoginInvites",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginInvites_Clubs_ClubId",
                table: "LoginInvites",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Clubs_ClubId",
                table: "UserAccounts",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginInvites_Clubs_ClubId",
                table: "LoginInvites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Clubs_ClubId",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_ClubId",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_LoginInvites_ClubId",
                table: "LoginInvites");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "LoginInvites");
        }
    }
}
