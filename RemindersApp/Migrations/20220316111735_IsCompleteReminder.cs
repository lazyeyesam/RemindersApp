using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemindersApp.Migrations
{
    public partial class IsCompleteReminder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Reminders",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Reminders");
        }
    }
}
