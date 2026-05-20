using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuzikSitesi.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalDueDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "CdKiralamalari",
                type: "TEXT",
                nullable: true);

            migrationBuilder.Sql("UPDATE \"CdKiralamalari\" SET \"DueDate\" = datetime(\"ApprovalDate\", '+15 days') WHERE \"ApprovalDate\" IS NOT NULL AND \"DueDate\" IS NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "CdKiralamalari");
        }
    }
}
