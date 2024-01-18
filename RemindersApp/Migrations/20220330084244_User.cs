using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemindersApp.Migrations
{
    public partial class User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ReminderLists",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReminderLists_UserId",
                table: "ReminderLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReminderLists_AspNetUsers_UserId",
                table: "ReminderLists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReminderLists_AspNetUsers_UserId",
                table: "ReminderLists");

            migrationBuilder.DropIndex(
                name: "IX_ReminderLists_UserId",
                table: "ReminderLists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReminderLists");
        }
    }
}
