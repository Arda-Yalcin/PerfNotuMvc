using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuzikSitesi.Migrations
{
    /// <inheritdoc />
    public partial class AddCdSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SepetKalemleri_Albumler_AlbumId",
                table: "SepetKalemleri");

            migrationBuilder.AlterColumn<int>(
                name: "AlbumId",
                table: "SepetKalemleri",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "CdId",
                table: "SepetKalemleri",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cdler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ad = table.Column<string>(type: "TEXT", nullable: true),
                    Foto = table.Column<string>(type: "TEXT", nullable: true),
                    GrupId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cdler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cdler_Gruplar_GrupId",
                        column: x => x.GrupId,
                        principalTable: "Gruplar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SepetKalemleri_CdId",
                table: "SepetKalemleri",
                column: "CdId");

            migrationBuilder.CreateIndex(
                name: "IX_Cdler_GrupId",
                table: "Cdler",
                column: "GrupId");

            migrationBuilder.AddForeignKey(
                name: "FK_SepetKalemleri_Albumler_AlbumId",
                table: "SepetKalemleri",
                column: "AlbumId",
                principalTable: "Albumler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SepetKalemleri_Cdler_CdId",
                table: "SepetKalemleri",
                column: "CdId",
                principalTable: "Cdler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SepetKalemleri_Albumler_AlbumId",
                table: "SepetKalemleri");

            migrationBuilder.DropForeignKey(
                name: "FK_SepetKalemleri_Cdler_CdId",
                table: "SepetKalemleri");

            migrationBuilder.DropTable(
                name: "Cdler");

            migrationBuilder.DropIndex(
                name: "IX_SepetKalemleri_CdId",
                table: "SepetKalemleri");

            migrationBuilder.DropColumn(
                name: "CdId",
                table: "SepetKalemleri");

            migrationBuilder.AlterColumn<int>(
                name: "AlbumId",
                table: "SepetKalemleri",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SepetKalemleri_Albumler_AlbumId",
                table: "SepetKalemleri",
                column: "AlbumId",
                principalTable: "Albumler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
