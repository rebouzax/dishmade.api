using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dishmade.infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantTableCrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RestaurantTables_Number",
                table: "RestaurantTables");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RestaurantTables",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantTables_Number",
                table: "RestaurantTables",
                column: "Number",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RestaurantTables_Number",
                table: "RestaurantTables");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RestaurantTables");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantTables_Number",
                table: "RestaurantTables",
                column: "Number",
                unique: true);
        }
    }
}
