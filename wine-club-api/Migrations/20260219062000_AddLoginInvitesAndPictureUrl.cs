using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wine_club_api.Migrations
{
    /// <inheritdoc />
    public partial class AddLoginInvitesAndPictureUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "UserAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoginInvites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginInvites", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoginInvites_Email",
                table: "LoginInvites",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginInvites");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "UserAccounts");
        }
    }
}
