using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace APYROPROJECTFINAL.Models
{
    public class StudentClassroomDB
    {

        [Key]
        public int Student_ClassroomID { get; set; }

        [Required]
        public int Classroom_Code { get; set; }

        [Required]
        [Display(Name = "Student Name")]
        public string Student_Name { get; set; }

        [Required]
        [Display(Name = "Student ID")]
        public string Student_ID { get; set; }

        [Required]
        public string Attendance_Start { get; set; }

        [Required]
        public string Attendance_End { get; set; }

        [Required]
        public string? Attendance_Time { get; set; }


        [Required]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Student Email")]
        public string StudentEmail { get; set; }

        [Required]
        [Display(Name = "Filename")]
        public string Filename { get; set; }


        [Required]
        [Display(Name = "Filepath")]
        public string Filepath { get; set; }

        [Required]
        [Display(Name = "Attendance Option")]
        public string Attendance_Option { get; set; }



        [Display(Name = "Absent")]
        public int? Absent { get; set; }


 
        [Display(Name = "Present")]
        public int? Present { get; set; }


      
        [Display(Name = "Late")]
        public int? Late { get; set; }



        [Display(Name = "Excused")]
        public int? Excused { get; set; }

        [Display(Name = "Tracker Status")]
        public string? Tracker {  get; set; }

    }
}
