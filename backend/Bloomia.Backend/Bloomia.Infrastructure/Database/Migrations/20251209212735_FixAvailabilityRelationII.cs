using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloomia.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixAvailabilityRelationII : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TherapistAvailabilities_Appointments_AppointmentId",
                table: "TherapistAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_TherapistAvailabilities_AppointmentId",
                table: "TherapistAvailabilities");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_TherapistAvailabilities_TherapistAvailabilityId",
                table: "Appointments",
                column: "TherapistAvailabilityId",
                principalTable: "TherapistAvailabilities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_TherapistAvailabilities_TherapistAvailabilityId",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistAvailabilities_AppointmentId",
                table: "TherapistAvailabilities",
                column: "AppointmentId",
                unique: true,
                filter: "[AppointmentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_TherapistAvailabilities_Appointments_AppointmentId",
                table: "TherapistAvailabilities",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }
    }
}
