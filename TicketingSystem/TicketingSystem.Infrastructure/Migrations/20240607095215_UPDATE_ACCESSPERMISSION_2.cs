using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_ACCESSPERMISSION_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AccessPermissions_Permission_UserId",
                table: "AccessPermissions",
                columns: new[] { "Permission", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccessPermissions_Permission_UserId",
                table: "AccessPermissions");
        }
    }
}
