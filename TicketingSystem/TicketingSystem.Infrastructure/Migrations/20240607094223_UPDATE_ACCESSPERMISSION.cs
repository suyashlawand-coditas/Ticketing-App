using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_ACCESSPERMISSION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccessPermissions_UserId_Permission",
                table: "AccessPermissions");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPermissions_UserId",
                table: "AccessPermissions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
