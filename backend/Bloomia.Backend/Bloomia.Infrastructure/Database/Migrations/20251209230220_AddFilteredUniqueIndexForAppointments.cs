using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloomia.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFilteredUniqueIndexForAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_TherapistAvailabilityId",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TherapistAvailabilityId",
                table: "Appointments",
                column: "TherapistAvailabilityId",
                unique: true,
                filter: "[IsDeleted]=0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_TherapistAvailabilityId",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TherapistAvailabilityId",
                table: "Appointments",
                column: "TherapistAvailabilityId",
                unique: true);
        }
    }
}
