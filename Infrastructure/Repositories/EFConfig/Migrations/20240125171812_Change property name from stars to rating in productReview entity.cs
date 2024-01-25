using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.EfConfig.Migrations
{
    /// <inheritdoc />
    public partial class ChangepropertynamefromstarstoratinginproductReviewentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stars",
                table: "ProductReviews",
                newName: "Rating");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "ProductReviews",
                newName: "Stars");
        }
    }
}
