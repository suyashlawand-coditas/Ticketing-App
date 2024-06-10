using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserCreationFKBugResolve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCreations_Users_UserCreationId",
                table: "UserCreations");

            migrationBuilder.CreateIndex(
                name: "IX_UserCreations_CreatedUserId",
                table: "UserCreations",
                column: "CreatedUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCreations_Users_CreatedUserId",
                table: "UserCreations",
                column: "CreatedUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCreations_Users_CreatedUserId",
                table: "UserCreations");

            migrationBuilder.DropIndex(
                name: "IX_UserCreations_CreatedUserId",
                table: "UserCreations");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCreations_Users_UserCreationId",
                table: "UserCreations",
                column: "UserCreationId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
