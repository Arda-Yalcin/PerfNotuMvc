using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuzikSitesi.Migrations
{
    /// <inheritdoc />
    public partial class AddAlbumRentalSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CdKiralamalari_Cdler_CdId",
                table: "CdKiralamalari");

            migrationBuilder.AlterColumn<int>(
                name: "CdId",
                table: "CdKiralamalari",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "AlbumId",
                table: "CdKiralamalari",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "CdKiralamalari",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_CdKiralamalari_AlbumId",
                table: "CdKiralamalari",
                column: "AlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_CdKiralamalari_Albumler_AlbumId",
                table: "CdKiralamalari",
                column: "AlbumId",
                principalTable: "Albumler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CdKiralamalari_Cdler_CdId",
                table: "CdKiralamalari",
                column: "CdId",
                principalTable: "Cdler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CdKiralamalari_Albumler_AlbumId",
                table: "CdKiralamalari");

            migrationBuilder.DropForeignKey(
                name: "FK_CdKiralamalari_Cdler_CdId",
                table: "CdKiralamalari");

            migrationBuilder.DropIndex(
                name: "IX_CdKiralamalari_AlbumId",
                table: "CdKiralamalari");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "CdKiralamalari");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "CdKiralamalari");

            migrationBuilder.AlterColumn<int>(
                name: "CdId",
                table: "CdKiralamalari",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CdKiralamalari_Cdler_CdId",
                table: "CdKiralamalari",
                column: "CdId",
                principalTable: "Cdler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
