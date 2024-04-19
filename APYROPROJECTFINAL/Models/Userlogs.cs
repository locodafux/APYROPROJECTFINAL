using System.ComponentModel.DataAnnotations;

namespace APYROPROJECTFINAL.Models
{
    public class Userlogs
    {


        [Key]
        public int userlogs {  get; set; }

        [Display(Name = "UserID")]
        public string UniqueId { get; set; }

        [Display(Name = "LogsEmail")]
        public string UserEmail { get; set; }

        [Display(Name = "Activity")]
        public string Email { get; set; }

        [Display(Name = "Timestamp")]
        public string Timestamp { get; set; }

    }
}
