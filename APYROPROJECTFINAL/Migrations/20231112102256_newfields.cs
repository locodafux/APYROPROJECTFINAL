using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APYROPROJECTFINAL.Migrations
{
    public partial class newfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Absent",
                table: "Student_Clasrooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Excused",
                table: "Student_Clasrooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Late",
                table: "Student_Clasrooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Present",
                table: "Student_Clasrooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Absent",
                table: "Student_Clasrooms");

            migrationBuilder.DropColumn(
                name: "Excused",
                table: "Student_Clasrooms");

            migrationBuilder.DropColumn(
                name: "Late",
                table: "Student_Clasrooms");

            migrationBuilder.DropColumn(
                name: "Present",
                table: "Student_Clasrooms");
        }
    }
}
