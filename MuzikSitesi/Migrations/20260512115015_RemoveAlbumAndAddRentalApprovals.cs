using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuzikSitesi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAlbumAndAddRentalApprovals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys = 0;");

            migrationBuilder.DropForeignKey(
                name: "FK_CdKiralamalari_Albumler_AlbumId",
                table: "CdKiralamalari");

            migrationBuilder.DropForeignKey(
                name: "FK_CdKiralamalari_Cdler_CdId",
                table: "CdKiralamalari");

            migrationBuilder.DropForeignKey(
                name: "FK_SepetKalemleri_Albumler_AlbumId",
                table: "SepetKalemleri");

            migrationBuilder.Sql("DELETE FROM \"SepetKalemleri\" WHERE \"CdId\" IS NULL;");
            migrationBuilder.Sql("DELETE FROM \"CdKiralamalari\" WHERE \"CdId\" IS NULL;");

            migrationBuilder.DropTable(
                name: "Albumler");

            migrationBuilder.DropIndex(
                name: "IX_SepetKalemleri_AlbumId",
                table: "SepetKalemleri");

            migrationBuilder.DropIndex(
                name: "IX_CdKiralamalari_AlbumId",
                table: "CdKiralamalari");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "SepetKalemleri");

            migrationBuilder.DropColumn(
                name: "AlbumId",
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

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalDate",
                table: "CdKiralamalari",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "CdKiralamalari",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnRequestDate",
                table: "CdKiralamalari",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReturnRequested",
                table: "CdKiralamalari",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("UPDATE \"CdKiralamalari\" SET \"IsApproved\" = 1, \"ApprovalDate\" = \"RentDate\" WHERE \"CdId\" IS NOT NULL;");

            migrationBuilder.AddForeignKey(
                name: "FK_CdKiralamalari_Cdler_CdId",
                table: "CdKiralamalari",
                column: "CdId",
                principalTable: "Cdler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql("PRAGMA foreign_keys = 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CdKiralamalari_Cdler_CdId",
                table: "CdKiralamalari");

            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "CdKiralamalari");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "CdKiralamalari");

            migrationBuilder.DropColumn(
                name: "ReturnRequestDate",
                table: "CdKiralamalari");

            migrationBuilder.DropColumn(
                name: "ReturnRequested",
                table: "CdKiralamalari");

            migrationBuilder.AddColumn<int>(
                name: "AlbumId",
                table: "SepetKalemleri",
                type: "INTEGER",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "Albumler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GrupId = table.Column<int>(type: "INTEGER", nullable: false),
                    Ad = table.Column<string>(type: "TEXT", nullable: true),
                    Foto = table.Column<string>(type: "TEXT", nullable: true),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albumler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albumler_Gruplar_GrupId",
                        column: x => x.GrupId,
                        principalTable: "Gruplar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SepetKalemleri_AlbumId",
                table: "SepetKalemleri",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_CdKiralamalari_AlbumId",
                table: "CdKiralamalari",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Albumler_GrupId",
                table: "Albumler",
                column: "GrupId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SepetKalemleri_Albumler_AlbumId",
                table: "SepetKalemleri",
                column: "AlbumId",
                principalTable: "Albumler",
                principalColumn: "Id");
        }
    }
}
