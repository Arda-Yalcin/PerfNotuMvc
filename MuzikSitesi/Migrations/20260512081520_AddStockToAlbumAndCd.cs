using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuzikSitesi.Migrations
{
    /// <inheritdoc />
    public partial class AddStockToAlbumAndCd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Cdler",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Albumler",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Cdler");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Albumler");
        }
    }
}
