using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APYROPROJECTFINAL.Migrations
{
    public partial class StudentClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Student_Clasrooms",
                columns: table => new
                {
                    Student_ClassroomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Classroom_Code = table.Column<int>(type: "int", nullable: false),
                    Student_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Student_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendance_Start = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendance_End = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendance_Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Filepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendance_Option = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student_Clasrooms", x => x.Student_ClassroomID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Student_Clasrooms");
        }
    }
}
