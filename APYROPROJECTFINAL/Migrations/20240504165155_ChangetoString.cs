using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APYROPROJECTFINAL.Migrations
{
    public partial class ChangetoString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DateTBL",
                table: "AttendanceReportDatanew",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DateTBL",
                table: "AttendanceReportDatanew",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
