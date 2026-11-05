using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloomia.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class MoveTherapistDocumentsOutOfWwwroot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE Documents SET FilePath = REPLACE(FilePath, '/uploads/documents/', 'documents/') " +
                "WHERE FilePath LIKE '/uploads/documents/%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE Documents SET FilePath = REPLACE(FilePath, 'documents/', '/uploads/documents/') " +
                "WHERE FilePath LIKE 'documents/%'");
        }
    }
}
