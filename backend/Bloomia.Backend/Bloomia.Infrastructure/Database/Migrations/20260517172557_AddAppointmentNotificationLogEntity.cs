using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloomia.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentNotificationLogEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentNotificationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    RecipientEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NotificationType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SentAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    AppointmentEntityId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentNotificationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentNotificationLogs_Appointments_AppointmentEntityId",
                        column: x => x.AppointmentEntityId,
                        principalTable: "Appointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppointmentNotificationLogs_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentNotificationLogs_AppointmentEntityId",
                table: "AppointmentNotificationLogs",
                column: "AppointmentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentNotificationLogs_AppointmentId",
                table: "AppointmentNotificationLogs",
                column: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentNotificationLogs");
        }
    }
}
