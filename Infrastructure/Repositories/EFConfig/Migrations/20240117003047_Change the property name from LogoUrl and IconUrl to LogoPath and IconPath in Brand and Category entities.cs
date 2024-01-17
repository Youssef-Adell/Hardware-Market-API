using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.EFConfig.Migrations
{
    /// <inheritdoc />
    public partial class ChangethepropertynamefromLogoUrlandIconUrltoLogoPathandIconPathinBrandandCategoryentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IconUrl",
                table: "ProductCategories",
                newName: "IconPath");

            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "ProductBrands",
                newName: "LogoPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IconPath",
                table: "ProductCategories",
                newName: "IconUrl");

            migrationBuilder.RenameColumn(
                name: "LogoPath",
                table: "ProductBrands",
                newName: "LogoUrl");
        }
    }
}
