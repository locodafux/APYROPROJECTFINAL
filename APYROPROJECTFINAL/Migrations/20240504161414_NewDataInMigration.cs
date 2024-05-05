using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APYROPROJECTFINAL.Migrations
{
    public partial class NewDataInMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceReportData",
                columns: table => new
                {
                    AttendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codeclass = table.Column<int>(type: "int", nullable: false),
                    StudentTBL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentIDTBL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttendanceDateTBL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusTBL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceReportData", x => x.AttendanceId);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceReportDatanew",
                columns: table => new
                {
                    AttendanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTBL = table.Column<int>(type: "int", nullable: false),
                    TimeTBL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeTBL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionTBL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceReportDatanew", x => x.AttendanceID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceReportData");

            migrationBuilder.DropTable(
                name: "AttendanceReportDatanew");
        }
    }
}
