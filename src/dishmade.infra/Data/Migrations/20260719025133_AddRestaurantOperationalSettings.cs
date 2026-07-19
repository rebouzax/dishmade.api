using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dishmade.infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantOperationalSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AcceptsQrCodeOrders",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AcceptsWaiterCall",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultServiceFeePercentage",
                table: "Restaurants",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptsQrCodeOrders",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "AcceptsWaiterCall",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "DefaultServiceFeePercentage",
                table: "Restaurants");
        }
    }
}
