using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeManager.API.Migrations.ApplicationDb
{
    public partial class addedEndTimeForTimeReg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndTime",
                table: "Timeregistration",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Timeregistration");
        }
    }
}
