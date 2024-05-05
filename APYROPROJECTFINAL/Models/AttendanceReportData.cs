using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace APYROPROJECTFINAL.Models
{
    public class AttendanceReportData
    {

        [Key]
        public int AttendanceId { get; set;}

        [Required]
        [Display(Name = "Classcode")]
        public int Codeclass {  get; set; }

        [Required]
        [Display(Name = "StudentTBL")]
        public string StudentTBL {  get; set; }

        [Required]
        [Display(Name = "StudentIDTBL")]
        public string StudentIDTBL { get; set; }

        [Required]
        [Display(Name = "AttendanceDateTBL")]
        public string AttendanceDateTBL { get; set; }


        [Required]
        [Display(Name = "StatusTBL")]
        public string StatusTBL {  get; set; }




    }
}
