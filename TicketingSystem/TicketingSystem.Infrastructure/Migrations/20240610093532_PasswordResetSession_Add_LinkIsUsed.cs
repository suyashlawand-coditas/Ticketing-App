using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PasswordResetSession_Add_LinkIsUsed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ForceToResetPassword",
                table: "PasswordResetSessions",
                newName: "LinkIsUsed");

            migrationBuilder.AddColumn<bool>(
                name: "ForcedToResetPassword",
                table: "PasswordResetSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForcedToResetPassword",
                table: "PasswordResetSessions");

            migrationBuilder.RenameColumn(
                name: "LinkIsUsed",
                table: "PasswordResetSessions",
                newName: "ForceToResetPassword");
        }
    }
}
