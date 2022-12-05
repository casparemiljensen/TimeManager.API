using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeManager.API.Migrations.ApplicationDb
{
    public partial class addedNewPropToTimeReg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Timeregistration",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Timeregistration");
        }
    }
}
