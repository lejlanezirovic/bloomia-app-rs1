using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloomia.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDirectChatEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ChatSessionId",
                table: "Messages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DirectChatId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAtUtc",
                table: "Messages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DirectChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TherapistId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirectChats_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DirectChats_Therapists_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Therapists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DirectChatId",
                table: "Messages",
                column: "DirectChatId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectChats_ClientId",
                table: "DirectChats",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectChats_TherapistId",
                table: "DirectChats",
                column: "TherapistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_DirectChats_DirectChatId",
                table: "Messages",
                column: "DirectChatId",
                principalTable: "DirectChats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_DirectChats_DirectChatId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "DirectChats");

            migrationBuilder.DropIndex(
                name: "IX_Messages_DirectChatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DirectChatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ModifiedAtUtc",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "ChatSessionId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
