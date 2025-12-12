using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloomia.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class MoodEntityInheritsBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Moods",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Moods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAtUtc",
                table: "Moods",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Moods");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Moods");

            migrationBuilder.DropColumn(
                name: "ModifiedAtUtc",
                table: "Moods");
        }
    }
}
