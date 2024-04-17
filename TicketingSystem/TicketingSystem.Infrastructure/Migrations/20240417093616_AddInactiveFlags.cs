using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInactiveFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "TicketResponses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "TicketResponses");
        }
    }
}
