using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Repositories.EfConfig.Migrations
{
    /// <inheritdoc />
    public partial class RemoveproductimagefromorderItementityandaddPaymentClientSecretpropertytoorderentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "PaymentClientSecret",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentClientSecret",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
