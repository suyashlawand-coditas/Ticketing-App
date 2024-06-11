using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ADD_PasswordResetSession_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PasswordResetSessions",
                columns: table => new
                {
                    PasswordResetSessionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedForUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ForceToResetPassword = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetSessions", x => x.PasswordResetSessionID);
                    table.ForeignKey(
                        name: "FK_PasswordResetSessions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_PasswordResetSessions_Users_CreatedForUserId",
                        column: x => x.CreatedForUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetSessions_CreatedById",
                table: "PasswordResetSessions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetSessions_CreatedForUserId",
                table: "PasswordResetSessions",
                column: "CreatedForUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordResetSessions");
        }
    }
}
