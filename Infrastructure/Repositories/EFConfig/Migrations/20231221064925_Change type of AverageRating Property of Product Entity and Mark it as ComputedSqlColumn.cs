using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.EFConfig.Migrations
{
    /// <inheritdoc />
    public partial class ChangetypeofAverageRatingPropertyofProductEntityandMarkitasComputedSqlColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "AverageRating",
                table: "Products",
                type: "real",
                nullable: false,
                computedColumnSql: "dbo.CalculateProductRate([Id])");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Products");
        }
    }
}
