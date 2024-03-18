using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APYROPROJECTFINAL.Migrations
{
    public partial class ClassroomDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassroomDBS",
                columns: table => new
                {
                    ClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Attendance_Option = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassCode = table.Column<int>(type: "int", nullable: false),
                    Educator_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendance_Start = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendance_End = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Days = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducatorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomDBS", x => x.ClassID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassroomDBS");
        }
    }
}
