using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Access_Permission_Add_Unique_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccessPermissions_UserId",
                table: "AccessPermissions");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPermissions_UserId_Permission",
                table: "AccessPermissions",
                columns: new[] { "UserId", "Permission" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccessPermissions_UserId_Permission",
                table: "AccessPermissions");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPermissions_UserId",
                table: "AccessPermissions",
                column: "UserId");
        }
    }
}
