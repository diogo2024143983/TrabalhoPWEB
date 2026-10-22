using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyHotels.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingStockUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "StockUpdated",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockUpdated",
                table: "Bookings");
        }
    }
}
