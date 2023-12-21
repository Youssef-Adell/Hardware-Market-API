using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.EFConfig.Migrations
{
    /// <inheritdoc />
    public partial class AddAverageRatingPropertyinProductEntityandmarkitasComputedSqlColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Products",
                type: "float",
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
