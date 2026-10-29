using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloomia.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_TherapistAvailabilityId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Until now cancellation was a soft delete, so existing deleted appointments are cancellations.
            // They keep IsDeleted = 1 and stay hidden; only appointments cancelled from now on remain visible.
            migrationBuilder.Sql("UPDATE [Appointments] SET [Status] = 1 WHERE [IsDeleted] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TherapistAvailabilityId",
                table: "Appointments",
                column: "TherapistAvailabilityId",
                unique: true,
                filter: "[IsDeleted]=0 AND [Status]<>1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_TherapistAvailabilityId",
                table: "Appointments");

            // Turn cancelled appointments back into soft deletes so the old unique index can be recreated.
            migrationBuilder.Sql("UPDATE [Appointments] SET [IsDeleted] = 1 WHERE [Status] = 1");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TherapistAvailabilityId",
                table: "Appointments",
                column: "TherapistAvailabilityId",
                unique: true,
                filter: "[IsDeleted]=0");
        }
    }
}
