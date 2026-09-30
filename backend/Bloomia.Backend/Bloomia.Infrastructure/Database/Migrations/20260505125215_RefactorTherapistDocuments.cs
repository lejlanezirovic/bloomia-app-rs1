using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloomia.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RefactorTherapistDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<int>(
                name: "TherapistId",
                table: "Documents",
                type: "int",
                nullable: true);

           
            migrationBuilder.Sql(@"
                UPDATE d
                SET d.[TherapistId] = t.[Id]
                FROM [Documents] d
                INNER JOIN [Therapists] t ON t.[DocumentId] = d.[Id];
            ");

          
            migrationBuilder.Sql(@"
                DELETE FROM [Documents] WHERE [TherapistId] IS NULL;
            ");

           
            migrationBuilder.AlterColumn<int>(
                name: "TherapistId",
                table: "Documents",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

          
            migrationBuilder.DropForeignKey(
                name: "FK_Therapists_Documents_DocumentId",
                table: "Therapists");

            migrationBuilder.DropIndex(
                name: "IX_Therapists_DocumentId",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Therapists");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Documents",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Documents",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileExtension",
                table: "Documents",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.Sql(@"
                UPDATE [Documents]
                SET [DocumentType] = CASE [DocumentType]
                    WHEN 'CV' THEN '1'
                    WHEN 'Diploma' THEN '2'
                    WHEN 'Certificate' THEN '3'
                    WHEN 'License' THEN '4'
                    ELSE '1'
                END
                WHERE [DocumentType] NOT LIKE '[0-9]%';
            ");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentType",
                table: "Documents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TherapistId",
                table: "Documents",
                column: "TherapistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Therapists_TherapistId",
                table: "Documents",
                column: "TherapistId",
                principalTable: "Therapists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Therapists_TherapistId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_TherapistId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "TherapistId",
                table: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "Therapists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FileExtension",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "DocumentType",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Therapists_DocumentId",
                table: "Therapists",
                column: "DocumentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Therapists_Documents_DocumentId",
                table: "Therapists",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
