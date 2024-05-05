using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APYROPROJECTFINAL.Migrations
{
    public partial class NewDatabasenow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "AttendanceReportDatanew",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EducatorClassCode",
                table: "AttendanceReportDatanew",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EducatorEmail",
                table: "AttendanceReportDatanew",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EducatorName",
                table: "AttendanceReportDatanew",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EducatorSection",
                table: "AttendanceReportDatanew",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "AttendanceReportDatanew");

            migrationBuilder.DropColumn(
                name: "EducatorClassCode",
                table: "AttendanceReportDatanew");

            migrationBuilder.DropColumn(
                name: "EducatorEmail",
                table: "AttendanceReportDatanew");

            migrationBuilder.DropColumn(
                name: "EducatorName",
                table: "AttendanceReportDatanew");

            migrationBuilder.DropColumn(
                name: "EducatorSection",
                table: "AttendanceReportDatanew");
        }
    }
}
