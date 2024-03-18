using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace APYROPROJECTFINAL.Models
{
    public class ClassroomDB
    {
        [Key]
        public int ClassID { get; set; }

        [Required]
        [Display(Name = "Attendance Option")]
        public string Attendance_Option { get; set; }

        [Required]
        public int ClassCode { get; set; }

        [Required]
        [Display(Name = "Educator Name")]
        public string Educator_Name { get; set; }

        [Required]
        [Display(Name = "Class Name")]
        public string ClassName { get; set; }

        [Required]
        public string Section { get; set; }

        [Required]
        [Display(Name = "Time Starts")]
        public string Attendance_Start { get; set; }

        [Required]
        [Display(Name = "Time Ends")]
        public string Attendance_End { get; set; }



        [Required]
        [RegularExpression("^(M|m|T|t|W|w|TH|th|F|f|S|s)+$", ErrorMessage = "Please enter valid days (M, T, W, TH, F, S).")]
        [Display(Name = "Days")]
        public string Days { get; set; }



        [Required]
        [Display(Name = "Educator Email")]
        public string EducatorEmail { get; set; }


        [Display(Name ="Tracker Status")]
        public string? TrackerStatus { get; set; }


    }
}
