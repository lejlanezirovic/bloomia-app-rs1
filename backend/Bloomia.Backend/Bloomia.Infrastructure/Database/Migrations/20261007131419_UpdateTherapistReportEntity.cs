using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloomia.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTherapistReportEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM TherapistReports");

            migrationBuilder.DropIndex(
                name: "IX_TherapistReports_TherapistId_Year_Month",
                table: "TherapistReports");

            migrationBuilder.DropColumn(
                name: "ActiveClientsCount",
                table: "TherapistReports");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "TherapistReports");

            migrationBuilder.DropColumn(
                name: "TotalReviews",
                table: "TherapistReports");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "TherapistReports");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "TherapistReports",
                type: "int",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFrom",
                table: "TherapistReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTo",
                table: "TherapistReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TherapistReports_ClientId",
                table: "TherapistReports",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistReports_TherapistId",
                table: "TherapistReports",
                column: "TherapistId");

            migrationBuilder.AddForeignKey(
                name: "FK_TherapistReports_Clients_ClientId",
                table: "TherapistReports",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM TherapistReports");

            migrationBuilder.DropForeignKey(
                name: "FK_TherapistReports_Clients_ClientId",
                table: "TherapistReports");

            migrationBuilder.DropIndex(
                name: "IX_TherapistReports_ClientId",
                table: "TherapistReports");

            migrationBuilder.DropIndex(
                name: "IX_TherapistReports_TherapistId",
                table: "TherapistReports");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "TherapistReports");

            migrationBuilder.DropColumn(
                name: "DateFrom",
                table: "TherapistReports");

            migrationBuilder.DropColumn(
                name: "DateTo",
                table: "TherapistReports");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "TherapistReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "TherapistReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActiveClientsCount",
                table: "TherapistReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalReviews",
                table: "TherapistReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TherapistReports_TherapistId_Year_Month",
                table: "TherapistReports",
                columns: new[] { "TherapistId", "Year", "Month" },
                unique: true);
        }
    }
}
