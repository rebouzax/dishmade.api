using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dishmade.infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuQrCodeToRestaurantTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMenuQrCodeEnabled",
                table: "RestaurantTables",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "MenuQrCodeEnabledAt",
                table: "RestaurantTables",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMenuQrCodeEnabled",
                table: "RestaurantTables");

            migrationBuilder.DropColumn(
                name: "MenuQrCodeEnabledAt",
                table: "RestaurantTables");
        }
    }
}
