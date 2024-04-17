using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketAssignment_Tickets_TicketId",
                table: "TicketAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAssignment_Users_AssignedUserId",
                table: "TicketAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketAssignment",
                table: "TicketAssignment");

            migrationBuilder.RenameTable(
                name: "TicketAssignment",
                newName: "TicketAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_TicketAssignment_TicketId",
                table: "TicketAssignments",
                newName: "IX_TicketAssignments_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketAssignment_AssignedUserId",
                table: "TicketAssignments",
                newName: "IX_TicketAssignments_AssignedUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketAssignments",
                table: "TicketAssignments",
                column: "TicketAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAssignments_Tickets_TicketId",
                table: "TicketAssignments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAssignments_Users_AssignedUserId",
                table: "TicketAssignments",
                column: "AssignedUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketAssignments_Tickets_TicketId",
                table: "TicketAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAssignments_Users_AssignedUserId",
                table: "TicketAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketAssignments",
                table: "TicketAssignments");

            migrationBuilder.RenameTable(
                name: "TicketAssignments",
                newName: "TicketAssignment");

            migrationBuilder.RenameIndex(
                name: "IX_TicketAssignments_TicketId",
                table: "TicketAssignment",
                newName: "IX_TicketAssignment_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketAssignments_AssignedUserId",
                table: "TicketAssignment",
                newName: "IX_TicketAssignment_AssignedUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketAssignment",
                table: "TicketAssignment",
                column: "TicketAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAssignment_Tickets_TicketId",
                table: "TicketAssignment",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAssignment_Users_AssignedUserId",
                table: "TicketAssignment",
                column: "AssignedUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
