using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace APYROPROJECTFINAL.Models
{
    public class AttendanceReportDatanew
    {

        [Key]
        public int AttendanceID { get; set;}


        [Required]
        [Display(Name = "DateTBL")]
        public string DateTBL { get; set; }

        [Required]
        [Display(Name = "TimeTBL ")]
        public string TimeTBL { get; set; }

        [Required]
        [Display(Name = "TypeTBL ")]
        public string TypeTBL { get; set; }

        [Required]
        [Display(Name = "DescriptionTBL ")]
        public string DescriptionTBL { get; set; }

        [Required]
        [Display(Name = "Educator Name ")]
        public string EducatorName { get; set; }

        [Required]
        [Display(Name = "Educator Email")]
        public string EducatorEmail {  get; set; }

        [Required]
        [Display(Name = "Section")]
        public string EducatorSection { get; set; }

        [Required]
        [Display(Name = "CourseName")]
        public string CourseName { get; set; }

        [Required]
        [Display(Name = "ClassCode")]
        public  int  EducatorClassCode { get; set; }

    }
}
