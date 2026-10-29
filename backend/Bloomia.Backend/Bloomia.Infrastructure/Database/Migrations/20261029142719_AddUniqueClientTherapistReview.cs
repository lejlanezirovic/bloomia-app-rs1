using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloomia.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueClientTherapistReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_ClientId",
                table: "Reviews");

            migrationBuilder.Sql(@"
                WITH Ranked AS (
                    SELECT [Id],
                           ROW_NUMBER() OVER (
                               PARTITION BY [ClientId], [TherapistId]
                               ORDER BY [CreatedAtUtc] DESC, [Id] DESC) AS [RowNumber]
                    FROM [Reviews]
                    WHERE [IsDeleted] = 0
                )
                UPDATE [Reviews]
                SET [IsDeleted] = 1, [ModifiedAtUtc] = SYSUTCDATETIME()
                WHERE [Id] IN (SELECT [Id] FROM Ranked WHERE [RowNumber] > 1);");

            migrationBuilder.Sql(@"
                UPDATE t
                SET [RatingAvg] = ISNULL((
                    SELECT ROUND(AVG(CAST(r.[Rating] AS float)), 1)
                    FROM [Reviews] r
                    WHERE r.[TherapistId] = t.[Id] AND r.[IsDeleted] = 0), 0)
                FROM [Therapists] t;");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ClientId_TherapistId",
                table: "Reviews",
                columns: new[] { "ClientId", "TherapistId" },
                unique: true,
                filter: "[IsDeleted]=0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_ClientId_TherapistId",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ClientId",
                table: "Reviews",
                column: "ClientId");
        }
    }
}
