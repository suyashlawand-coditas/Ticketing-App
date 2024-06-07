using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Access_Permission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GrantedById",
                table: "AccessPermissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AccessPermissions_GrantedById",
                table: "AccessPermissions",
                column: "GrantedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessPermissions_Users_GrantedById",
                table: "AccessPermissions",
                column: "GrantedById",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessPermissions_Users_GrantedById",
                table: "AccessPermissions");

            migrationBuilder.DropIndex(
                name: "IX_AccessPermissions_GrantedById",
                table: "AccessPermissions");

            migrationBuilder.DropColumn(
                name: "GrantedById",
                table: "AccessPermissions");
        }
    }
}
