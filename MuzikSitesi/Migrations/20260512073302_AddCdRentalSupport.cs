using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuzikSitesi.Migrations
{
    /// <inheritdoc />
    public partial class AddCdRentalSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CdKiralamalari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: false),
                    CdId = table.Column<int>(type: "INTEGER", nullable: false),
                    RentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsReturned = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CdKiralamalari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CdKiralamalari_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CdKiralamalari_Cdler_CdId",
                        column: x => x.CdId,
                        principalTable: "Cdler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CdKiralamalari_AppUserId",
                table: "CdKiralamalari",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CdKiralamalari_CdId",
                table: "CdKiralamalari",
                column: "CdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CdKiralamalari");
        }
    }
}
