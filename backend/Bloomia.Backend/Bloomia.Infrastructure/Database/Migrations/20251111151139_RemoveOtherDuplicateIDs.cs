using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloomia.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOtherDuplicateIDs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "SelfTestResults",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SelfTestResults",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAtUtc",
                table: "SelfTestResults",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "SelfTestQuestions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SelfTestQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAtUtc",
                table: "SelfTestQuestions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "SelfTestAnswers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SelfTestAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAtUtc",
                table: "SelfTestAnswers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "SelfTestResults");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SelfTestResults");

            migrationBuilder.DropColumn(
                name: "ModifiedAtUtc",
                table: "SelfTestResults");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "SelfTestQuestions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SelfTestQuestions");

            migrationBuilder.DropColumn(
                name: "ModifiedAtUtc",
                table: "SelfTestQuestions");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "SelfTestAnswers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SelfTestAnswers");

            migrationBuilder.DropColumn(
                name: "ModifiedAtUtc",
                table: "SelfTestAnswers");
        }
    }
}
