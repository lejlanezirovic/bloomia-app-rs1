using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloomia.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixAppointmentNotificationLogRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentNotificationLogs_Appointments_AppointmentEntityId",
                table: "AppointmentNotificationLogs");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentNotificationLogs_AppointmentEntityId",
                table: "AppointmentNotificationLogs");

            migrationBuilder.DropColumn(
                name: "AppointmentEntityId",
                table: "AppointmentNotificationLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentEntityId",
                table: "AppointmentNotificationLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentNotificationLogs_AppointmentEntityId",
                table: "AppointmentNotificationLogs",
                column: "AppointmentEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentNotificationLogs_Appointments_AppointmentEntityId",
                table: "AppointmentNotificationLogs",
                column: "AppointmentEntityId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }
    }
}
