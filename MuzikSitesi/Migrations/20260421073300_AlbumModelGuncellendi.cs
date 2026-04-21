using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuzikSitesi.Migrations
{
    /// <inheritdoc />
    public partial class AlbumModelGuncellendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albumler_Gruplar_GruplarId",
                table: "Albumler");

            migrationBuilder.RenameColumn(
                name: "GruplarId",
                table: "Albumler",
                newName: "GrupId");

            migrationBuilder.RenameIndex(
                name: "IX_Albumler_GruplarId",
                table: "Albumler",
                newName: "IX_Albumler_GrupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albumler_Gruplar_GrupId",
                table: "Albumler",
                column: "GrupId",
                principalTable: "Gruplar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albumler_Gruplar_GrupId",
                table: "Albumler");

            migrationBuilder.RenameColumn(
                name: "GrupId",
                table: "Albumler",
                newName: "GruplarId");

            migrationBuilder.RenameIndex(
                name: "IX_Albumler_GrupId",
                table: "Albumler",
                newName: "IX_Albumler_GruplarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albumler_Gruplar_GruplarId",
                table: "Albumler",
                column: "GruplarId",
                principalTable: "Gruplar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
