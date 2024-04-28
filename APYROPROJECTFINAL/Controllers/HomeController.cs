using APYROPROJECTFINAL.Areas.Identity.Data;
using APYROPROJECTFINAL.Areas.Identity.Pages.Account;
using APYROPROJECTFINAL.Data;
using APYROPROJECTFINAL.Migrations;
using APYROPROJECTFINAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Userlogs = APYROPROJECTFINAL.Models.Userlogs;

namespace APYROPROJECTFINAL.Controllers
{

    [Authorize (Roles = "User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnviroment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AuthDbContext _context;
        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment hostEnvironment, UserManager<ApplicationUser>userManager, AuthDbContext context)
        {
            _logger = logger;
            this._userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _hostEnviroment = hostEnvironment;

        }









        public async Task<IActionResult> Index()
        {


            var user = await _userManager.GetUserAsync(User);

            ViewData["UserID"] = _userManager.GetUserId(this.User);

         
            
            await LogUserActivityAsync();
            


            if (user != null)
            {
                var userAttendanceData = await _context.Student_Clasrooms
                    .Where(sc => sc.StudentEmail == user.UserName)
                    .Select(sc => new
                    {
                        sc.Present,
                        sc.Absent,
                        sc.Late,
                        sc.Excused
                    })
                    .ToListAsync();

                if (userAttendanceData != null && userAttendanceData.Any())
                {
                    var presentCount = userAttendanceData.Sum(sc => sc.Present ?? 0);
                    var absentCount = userAttendanceData.Sum(sc => sc.Absent ?? 0);

                    var lateCount = userAttendanceData.Sum(sc => sc.Late ?? 0);
                    //var excusedCount = userAttendanceData.Sum(sc => sc.Excused ?? 0);
                    // Use ViewBag to pass counts to the view
                    ViewBag.PresentCount = presentCount;
                    ViewBag.AbsentCount = absentCount;
                    ViewBag.LateCount = lateCount;
                    //ViewBag.LateCount = lateCount;
                    //ViewBag.ExcusedCount = excusedCount;

                    var newDataForTuesday = new AttendanceData
                    {
                        Day = "Monday",
                        Present = presentCount,
                        Absent = absentCount,
                        Late = lateCount
                    };

                    var data = new List<AttendanceData>
                      {
                           new AttendanceData { Day = "Saturday", Absent = 0, Present = 0, Late = 0 },
                           new AttendanceData { Day = "Sunday", Absent = 0, Present = 0, Late = 0 },
                           newDataForTuesday,
                       };

                             ViewBag.AttendanceChartData = data;




                    List<DoughnutChartData> ChartPoints = new List<DoughnutChartData>
                        {
                             new DoughnutChartData { Browser= "Present", Users= ViewBag.PresentCount ?? 0, DataLabelMappingName= $"Present: {ViewBag.PresentCount ?? 0}%" },
                             new DoughnutChartData { Browser= "Absent", Users= ViewBag.AbsentCount ?? 0, DataLabelMappingName= $"Absent: {ViewBag.AbsentCount ?? 0}%" },
                             new DoughnutChartData { Browser= "Late", Users=  ViewBag.LateCount ?? 0, DataLabelMappingName= $"Late: {ViewBag.LateCount ?? 0}%" }
                        };

                               ViewBag.ChartPoints = ChartPoints;









                       }




                        else
                              {
                                var data = new List<AttendanceData>
                                      {
                                         new AttendanceData { Day = "Sunday", Absent = 0, Present = 0, Late = 0 },
                                         new AttendanceData { Day = "Monday", Absent = 0, Present = 0, Late = 0 },
                                         new AttendanceData { Day = "Tuesday", Absent = 0, Present = 0, Late = 0 },
                                         new AttendanceData { Day = "Wednesday", Absent = 0, Present = 0, Late = 0 },
                                         new AttendanceData { Day = "Thursday", Absent = 0, Present = 0, Late = 0 },
                                         new AttendanceData { Day = "Friday", Absent = 0, Present = 0, Late = 0 },
                                         new AttendanceData { Day = "Saturday", Absent = 0, Present = 0, Late = 0 },
                                 };

                                  ViewBag.AttendanceChartData = data;




                    List<DoughnutChartData> ChartPoints = new List<DoughnutChartData>
                            {
                                new DoughnutChartData { Browser= "Present", Users= 0, DataLabelMappingName= "Present: 0%" },
                                //new DoughnutChartData { Browser= "Late", Users= 0, DataLabelMappingName= "Late: 0%" },
                                new DoughnutChartData { Browser= "Absent", Users= 0, DataLabelMappingName= "Absent: 0%" },
                                //new DoughnutChartData { Browser= "Excuse", Users= 0, DataLabelMappingName= "Excuse: 0%" },
                            };
                    ViewBag.ChartPoints = ChartPoints;

                }





            }













            















            //if (user != null)
            //{
            //    var userAttendanceData = await _context.Student_Clasrooms
            //        .Where(sc => sc.StudentEmail == user.UserName)
            //        .Select(sc => new
            //        {
            //            sc.Present,
            //            sc.Absent,
            //            sc.Late,
            //            sc.Excused
            //        })
            //        .FirstOrDefaultAsync();

            //    if (userAttendanceData != null)
            //    {
            //        var presentCount = userAttendanceData.Present;
            //        var absentCount = userAttendanceData.Absent;
            //        var lateCount = userAttendanceData.Late;
            //        var excusedCount = userAttendanceData.Excused;
            //        // Use ViewBag to pass counts to the view
            //        ViewBag.PresentCount = presentCount;
            //        ViewBag.AbsentCount = absentCount;
            //        ViewBag.LateCount = lateCount;
            //        ViewBag.ExcusedCount = excusedCount;

            //        var newDataFortuesday = new AttendanceData
            //        {
            //            Day = "Monday",
            //            Present = ViewBag.PresentCount ?? 0, // Use 0 if PresentCount is null
            //            Absent = ViewBag.AbsentCount ?? 0,   // Use 0 if AbsentCount is null
            //            Late = ViewBag.LateCount ?? 0        // Use 0 if LateCount is null

            //        };

            //        var data = new List<AttendanceData>
            //            {

            //                 new AttendanceData { Day = "Saturday", Absent = 0, Present = 0, Late = 0 },
            //                 new AttendanceData { Day = "Sunday", Absent = 0, Present = 0, Late = 0 },
            //                 newDataFortuesday,
            //             };

            //        ViewBag.AttendanceChartData = data;





            //        List<DoughnutChartData> ChartPoints = new List<DoughnutChartData>
            //        {
            //              new DoughnutChartData { Browser= "Present", Users= presentCount ?? 0, DataLabelMappingName= $"Present: {presentCount ?? 0}%" },
            //              //new DoughnutChartData { Browser= "Late", Users= lateCount ?? 0, DataLabelMappingName= $"Late: {lateCount ?? 0}%" },
            //              new DoughnutChartData { Browser= "Absent", Users= absentCount ?? 0, DataLabelMappingName= $"Absent: {absentCount ?? 0}%" },
            //              //new DoughnutChartData { Browser= "Excuse", Users= excusedCount ?? 0, DataLabelMappingName= $"Excuse: {excusedCount ?? 0}%" },
            //         };
            //         ViewBag.ChartPoints = ChartPoints;



            //        //Fix Attendance Charts based it on total of absents,presents,lates,excused on weekdays
            //        //Or based it on each classroom records 




            //    }
            //    else
            //    {

            //        var data = new List<AttendanceData>
            //              {
            //                    new AttendanceData { Day = "Sunday", Absent = 0, Present = 0, Late = 0 },
            //                    new AttendanceData { Day = "Monday", Absent = 0, Present = 0, Late = 0 },
            //                    new AttendanceData { Day = "Tuesday", Absent = 0, Present = 0, Late = 0 },
            //                    new AttendanceData { Day = "Wednesday", Absent = 0, Present = 0, Late = 0 },
            //                    new AttendanceData { Day = "Thursday", Absent = 0, Present = 0, Late = 0 },
            //                    new AttendanceData { Day = "Friday", Absent = 0, Present = 0, Late = 0 },
            //                    new AttendanceData { Day = "Saturday", Absent = 0, Present = 0, Late = 0 },
            //               };

            //        ViewBag.AttendanceChartData = data;




            //        List<DoughnutChartData> ChartPoints = new List<DoughnutChartData>
            //        {
            //            new DoughnutChartData { Browser= "Present", Users= 0, DataLabelMappingName= "Present: 0%" },
            //            new DoughnutChartData { Browser= "Late", Users= 0, DataLabelMappingName= "Late: 0%" },
            //            new DoughnutChartData { Browser= "Absent", Users= 0, DataLabelMappingName= "Absent: 0%" },
            //            new DoughnutChartData { Browser= "Excuse", Users= 0, DataLabelMappingName= "Excuse: 0%" },
            //        };
            //        ViewBag.ChartPoints = ChartPoints;



            //    }





            //}







            //var data = new List<AttendanceData>
            //              {
            //                    new AttendanceData { Day = "Sunday", Absent = 0, Present = 0, Late = 0 },
            //                    new AttendanceData { Day = "Monday", Absent = 5, Present = 10, Late = 5 },
            //                    new AttendanceData { Day = "Tuesday", Absent = 15, Present = 20, Late = 10 },
            //                    new AttendanceData { Day = "Wednesday", Absent = 25, Present = 35, Late = 15 },
            //                    new AttendanceData { Day = "Thursday", Absent = 35, Present = 50, Late = 20 },
            //                    new AttendanceData { Day = "Friday", Absent = 45, Present = 65, Late = 25 },
            //                    new AttendanceData { Day = "Saturday", Absent = 50, Present = 80, Late = 30 },
            //               };

            //ViewBag.AttendanceChartData = data;




            //List<DoughnutChartData> ChartPoints = new List<DoughnutChartData>
            //        {
            //            new DoughnutChartData { Browser= "Present", Users= 40.3, DataLabelMappingName= "Present: 40.3%" },
            //            new DoughnutChartData { Browser= "Late", Users= 10.6, DataLabelMappingName= "Late: 10.6%" },
            //            new DoughnutChartData { Browser= "Absent", Users= 15.0, DataLabelMappingName= "Absent: 15.0%" },
            //            new DoughnutChartData { Browser= "Excuse", Users= 5.7, DataLabelMappingName= "Excuse: 5.7%" },
            //        };
            //ViewBag.ChartPoints = ChartPoints;










            if (user != null)
            {
                var userJoinedClasses = await _context.Student_Clasrooms
                    .Where(sc => sc.StudentEmail == user.Email)
                    .Join(
                        _context.ClassroomDBS,
                        studentClass => studentClass.Classroom_Code,
                        classroom => classroom.ClassCode,
                        (studentClass, classroom) => classroom
                    )
                    .ToListAsync();





                return View(userJoinedClasses);
            }










            return RedirectToAction("Login");
        }





        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClassroomDBS == null)
            {
                return NotFound();
            }

            var classroomDB = await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.ClassID == id);

            if (classroomDB == null)
            {
                return NotFound();
            }

            ViewBag.Classid1 = id;

            var studentClassroomsDB = await _context.Student_Clasrooms
                .Where(s => s.Classroom_Code == classroomDB.ClassCode)
                .ToListAsync();


            var viewModel = new ClassroomViewModel
            {
                ClassroomDB = classroomDB,
                Students = studentClassroomsDB
            };




            if (classroomDB != null)
            {
                var studentClassroom = await _context.Student_Clasrooms
                    .FirstOrDefaultAsync(s => s.Classroom_Code == classroomDB.ClassCode && classroomDB.TrackerStatus == s.Tracker);

                if (studentClassroom != null)
                {
                    ViewBag.Tracker = studentClassroom.Tracker;
                }
            }






            return View(viewModel);
        }




        public IActionResult FaceDataPrivacy()
        {
            return View();
        }






        //ALL STUDENTS CAN BE TRACK IN WEBCAM


        //public async Task<JsonResult> GetTrackerData()
        //{
        //    var classroomDB = await _context.ClassroomDBS
        //                                     .Select(c => c.ClassCode)
        //                                     .FirstOrDefaultAsync();

        //    var user = await _userManager.GetUserAsync(User);

        //    if (user != null)
        //    {
        //        var studentClassroomCodes = await _context.Student_Clasrooms
        //                                                  .Where(s => s.StudentEmail == user.UserName)
        //                                                  .Select(s => s.Classroom_Code)
        //                                                  .ToListAsync();

        //        var data = await _context.Student_Clasrooms
        //                                  .Where(s => studentClassroomCodes.Contains(s.Classroom_Code) && s.Classroom_Code == classroomDB)
        //                                  .Select(s => new
        //                                  {
        //                                      ClassCode = s.Classroom_Code,
        //                                      StudentName = s.Student_Name,
        //                                      ImageFile = s.Filename,
        //                                  })
        //                                  .ToListAsync();

        //        return Json(data);
        //    }

        //    return Json(null);
        //}



        public async Task<IActionResult> GetTrackerData()
        {
            var classroomDB = await _context.ClassroomDBS.FirstOrDefaultAsync();
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var studentClassrooms = await _context.Student_Clasrooms
                    .Where(s => s.StudentEmail == user.UserName)
                    .Select(s => new
                    {
                        ClassCode = s.Classroom_Code,
                        StudentName = s.Student_Name,
                        ImageFile = s.Filename
                    })
                    .ToListAsync();

                return Json(studentClassrooms);
            }

            return Json(null);

        }





        public IActionResult StudentLogout()
        {
            return View();
        }




        //Check Student Face Registration Modal
        public async Task<IActionResult> CheckStudentData()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null )
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.EmailStudent == user.Email);
                if (student != null && string.IsNullOrEmpty(student.FileName))
                {
                    ViewBag.Message = "Null";
                }
            }

            return View();
        }





        public async Task<IActionResult> ProcessTrackerAsync([FromBody] FetchTracker tracker)
        {
            var studentName = tracker.studentName;
            var classCode = tracker.classCode;
            var classidss = tracker.classId;

            var classroomDB = await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.ClassID == classidss);

            if (classroomDB == null)
            {
                return NotFound();
            }
            var classcoding = classroomDB.ClassCode;




            // Retrieve the student based on provided studentName and classCode
            var student = await _context.Student_Clasrooms
         .FirstOrDefaultAsync(s => s.Student_Name == studentName && s.Classroom_Code == classCode && s.Classroom_Code == classcoding);


            if (student != null)
            {
                // Update the Status field to "Present"




                //student.Status = "Present";



                TimeZoneInfo phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");

                // Convert the UTC time to Philippines Standard Time
                DateTime phTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);

                // Update the Attendance_Time field with the Philippines time
                student.Attendance_Time = phTime.ToString("dd-MM-yyyy hh:mm tt");

                // Convert student.Attendance_Start to DateTime
                DateTime attendanceStart = DateTime.ParseExact(student.Attendance_Start, "HH:mm", CultureInfo.InvariantCulture);

                if (phTime > attendanceStart.AddMinutes(15))
                {
                    student.Status = "Late";
                    student.Late = (student.Late ?? 0) + 1;
                }
                else
                {
                    student.Status = "Present";
                    student.Present = (student.Present ?? 0) + 1;
                }




                //student.Present = (student.Present ?? 0) + 1;






                // student.Absent = (student.Absent ?? 0) + 15;
                // student.Late = (student.Late ?? 0) + 11;

                // Save changes to the database
                await _context.SaveChangesAsync();

                var responseData = new { Message = "Data received successfully. Student: " + studentName + ", Class Code: " + classCode + ". Status updated to 'Present'" };

                return Json(responseData);
            }
            else
            {
                var responseData = new { Message = "Student data not found or does not match." };
                return Json(responseData);
            }

        }




 
        public ActionResult joinclass()
        {
            
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CheckCode11(int code)
        {
           
                var user = await _userManager.GetUserAsync(User);
                var student = await _context.Students.FirstOrDefaultAsync(s => s.EmailStudent == user.UserName);



             
                    string studentName = student.FirstName + student.LastName;
                    string studentID = student.IDnumber;
                    string studentEmail = student.EmailStudent;
                    var imagesFolder = Path.Combine(_hostEnviroment.WebRootPath, "images/Verified");
                    string filename = student.FileName ?? "image_638491651343075578.jpg"; // Assuming the default image name is "image_638491651343075578.jpg"
                    string filepath = Path.Combine(imagesFolder, filename);





                    var isUserAlreadyAMember = await _context.Student_Clasrooms
                        .AnyAsync(sc => sc.StudentEmail == studentEmail && sc.Classroom_Code == code);

                    if (isUserAlreadyAMember == false)
                    {
                        var classroomRecord = await _context.ClassroomDBS.FirstOrDefaultAsync(c => c.ClassCode == code);

                        if (classroomRecord != null)
                        {
                            string AttendanceStart = classroomRecord.Attendance_Start;
                            string AttendancEnd = classroomRecord.Attendance_End;
                            string AttendanceOption = classroomRecord.Attendance_Option;

                            AddStudentToClassroomDB(code, studentName, studentID, AttendanceStart, AttendancEnd, "Pending Time", "Accept", studentEmail, filename, filepath, AttendanceOption);

                            
                           
                            return Json(new { success = true, message = "Joined updated" });
                        }
                    }
          
                
            
        

            return Json(new { success = false, message = "not Joined" + " " + isUserAlreadyAMember + code});
        }




        //[HttpPost]
        //public async Task<IActionResult> joinclassAsync(int classcode)
        //{

        //    var classCode = _context.ClassroomDBS.FirstOrDefault(c => c.ClassCode == classcode);
        //    var user = await _userManager.GetUserAsync(User);

        //    if (classCode != null && user is Student student)
        //    {
        //        string studentName = student.FirstName + student.LastName; // Replace with the actual property name in your IdentityUser model
        //        string studentID = student.IDnumber; // Replace with the actual property name in your IdentityUser model
        //        string studentEmail = student.EmailStudent;
        //        string filename = student.FileName;
        //        string filepath = student.FilePath;

        //        // Check if the user is already a member of this class
        //        var isUserAlreadyAMember = await _context.Student_Clasrooms
        //            .AnyAsync(sc => sc.Student_ID == studentID && sc.Classroom_Code == classCode.ClassCode);

        //        if (!isUserAlreadyAMember)
        //        {
        //            var classroomRecord = await _context.ClassroomDBS.FirstOrDefaultAsync(c => c.ClassCode == classcode);

        //            if (classroomRecord != null)
        //            {
        //                string AttendanceStart = classroomRecord.Attendance_Start;
        //                string AttendancEnd = classroomRecord.Attendance_End;
        //                string AttendanceOption = classroomRecord.Attendance_Option;

        //                AddStudentToClassroomDB(classcode, studentName, studentID, AttendanceStart, AttendancEnd, "Pending Time", "Accept", studentEmail, filename, filepath, AttendanceOption);

        //                // Set a ViewBag to indicate correctness
        //                //ViewBag.IsCodeCorrect = true;

        //                return RedirectToAction("Index");
        //            }
        //        }

        //    }

        //    return View();
        //}



        //public IActionResult joinclass(int classcode)
        //{
        //    var classCode = _context.ClassroomDBS.FirstOrDefault(c => c.ClassCode == classcode);
        //    var user = _userManager.GetUserAsync(User).Result;

        //    if (classCode != null && user is Student student)
        //    {
        //        string studentName = student.FirstName + student.LastName; // Replace with the actual property name in your IdentityUser model
        //        string studentID = student.IDnumber; // Replace with the actual property name in your IdentityUser model
        //        string studentEmail = student.EmailStudent;
        //        string filename = student.FileName;
        //        string filepath = student.FilePath;

        //        // Check if the user is already a member of this class
        //        var isUserAlreadyAMember = _context.Student_Clasrooms
        //            .Any(sc => sc.Student_ID == studentID && sc.Classroom_Code == classCode.ClassCode);

        //        if (!isUserAlreadyAMember)
        //        {
        //            var classroomRecord = _context.ClassroomDBS.FirstOrDefault(c => c.ClassCode == classcode);

        //            if (classroomRecord != null)
        //            {
        //                string AttendanceStart = classroomRecord.Attendance_Start;
        //                string AttendancEnd = classroomRecord.Attendance_End;
        //                string AttendanceOption = classroomRecord.Attendance_Option;

        //                AddStudentToClassroomDB(classcode, studentName, studentID, AttendanceStart, AttendancEnd, "Pending Time", "Accept", studentEmail, filename, filepath, AttendanceOption);

        //                // Set a ViewBag to indicate correctness
        //                // ViewBag.IsCodeCorrect = true;

        //                return RedirectToAction("Index");
        //            }
        //        }
        //    }

        //    return View();
        //}















        //public IActionResult CheckCode()
        //{
        //    return View();
        //}




        //[HttpPost]
        //public async Task<IActionResult> CheckCode11(int code)
        //{
        //    var classCode = await _context.ClassroomDBS.FirstOrDefaultAsync(c => c.ClassCode == code);
        //    var classCodes = await _context.ClassroomDBS.ToListAsync();
        //    ViewBag.ClassCodes = classCodes;

        //    if (classCode != null)
        //    {
        //        var user = await _userManager.GetUserAsync(User);
        //        var student = await _context.Students.FirstOrDefaultAsync(s => s.EmailStudent == user.UserName);

        //        if (student != null)
        //        {
        //            string studentName = student.FirstName + student.LastName;
        //            string studentID = student.IDnumber;
        //            string studentEmail = student.EmailStudent;
        //            string filename = student.FileName;
        //            string filepath = student.FilePath;

        //            var isUserAlreadyAMember = await _context.Student_Clasrooms
        //                .AnyAsync(sc => sc.Student_ID == studentID && sc.Classroom_Code == classCode.ClassCode);

        //            if (!isUserAlreadyAMember)
        //            {
        //                var classroomRecord = await _context.ClassroomDBS.FirstOrDefaultAsync(c => c.ClassCode == code);

        //                if (classroomRecord != null)
        //                {
        //                    string AttendanceStart = classroomRecord.Attendance_Start;
        //                    string AttendancEnd = classroomRecord.Attendance_End;
        //                    string AttendanceOption = classroomRecord.Attendance_Option;

        //                    AddStudentToClassroomDB(code, studentName, studentID, AttendanceStart, AttendancEnd, "Pending Time", "Accept", studentEmail, filename, filepath, AttendanceOption);

        //                    return Json(new { success = true, message = "Joined updated" });
        //                }
        //            }
        //            else
        //            {
        //                ViewBag.IsCodeCorrect = false;
        //                ViewBag.ErrorMessage = "You are already a member of this class.";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.IsCodeCorrect = false;
        //    }

        //    return Json(new { success = false, message = "not Joined" });
        //}



        //[HttpPost]
        //public async Task<IActionResult> CheckCode11Async(int code)
        //{
        //    var classCode = _context.ClassroomDBS.FirstOrDefault(c => c.ClassCode == code);

        //    var user = await _userManager.GetUserAsync(User);


        //    if (classCode != null && user is Student student)
        //    {

        //        string studentName = student.FirstName + student.LastName; // Replace with the actual property name in your IdentityUser model
        //        string studentID = student.IDnumber; // Replace with the actual property name in your IdentityUser model
        //        string studentEmail = student.EmailStudent;
        //        string filename = student.FileName;
        //        string filepath = student.FilePath;

        //        // Check if the user is already a member of this class
        //        var isUserAlreadyAMember = await _context.Student_Clasrooms
        //            .AnyAsync(sc => sc.Student_ID == studentID && sc.Classroom_Code == classCode.ClassCode);

        //        if (!isUserAlreadyAMember)
        //        {
        //            var classroomRecord = await _context.ClassroomDBS.FirstOrDefaultAsync(c => c.ClassCode == code);

        //            if (classroomRecord != null)
        //            {
        //                string AttendanceStart = classroomRecord.Attendance_Start;
        //                string AttendancEnd = classroomRecord.Attendance_End;
        //                string AttendanceOption = classroomRecord.Attendance_Option;

        //                AddStudentToClassroomDB(code, studentName, studentID, AttendanceStart, AttendancEnd, "Pending Time", "Accept", studentEmail, filename, filepath, AttendanceOption);

        //                // Set a ViewBag to indicate correctness
        //                ViewBag.IsCodeCorrect = true;

        //                return RedirectToAction("Index");
        //            }
        //        }
        //        else
        //        {
        //            // User is already a member, show an error message
        //            ViewBag.IsCodeCorrect = false;
        //            ViewBag.ErrorMessage = "You are already a member of this class.";

        //        }
        //    }
        //    else
        //    {
        //        // Code is incorrect, set a ViewBag to indicate incorrectness
        //        ViewBag.IsCodeCorrect = false;

        //    }
        //    return View();

        //}


        //[HttpPost]
        //public async Task<IActionResult> joinclassAsync(int code)
        //{


        //    var classCode = _context.ClassroomDBS.FirstOrDefault(c => c.ClassCode == code);
        //    var classCodes = await _context.ClassroomDBS.ToListAsync();


        //    var user = await _userManager.GetUserAsync(User);


        //    if (classCode != null && user is Student student)
        //    {

        //        string studentName = student.FirstName + student.LastName; // Replace with the actual property name in your IdentityUser model
        //        string studentID = student.IDnumber; // Replace with the actual property name in your IdentityUser model
        //        string studentEmail = student.EmailStudent;
        //        string filename = student.FileName;
        //        string filepath = student.FilePath;

        //        // Check if the user is already a member of this class
        //        var isUserAlreadyAMember = await _context.Student_Clasrooms
        //            .AnyAsync(sc => sc.Student_ID == studentID && sc.Classroom_Code == classCode.ClassCode);

        //        if (!isUserAlreadyAMember)
        //        {
        //            var classroomRecord = await _context.ClassroomDBS.FirstOrDefaultAsync(c => c.ClassCode == code);

        //            if (classroomRecord != null)
        //            {
        //                string AttendanceStart = classroomRecord.Attendance_Start;
        //                string AttendancEnd = classroomRecord.Attendance_End;
        //                string AttendanceOption = classroomRecord.Attendance_Option;

        //                AddStudentToClassroomDB(code, studentName, studentID, AttendanceStart, AttendancEnd, "Pending Time", "Accept", studentEmail, filename, filepath, AttendanceOption);


        //                return RedirectToAction("Index");
        //            }
        //        }
        //        else
        //        {
        //            // User is already a member, show an error message
        //            //ViewBag.IsCodeCorrect = false;
        //            //ViewBag.ErrorMessage = "You are already a member of this class.";
        //        }
        //    }
        //    else
        //    {
        //        // Code is incorrect, set a ViewBag to indicate incorrectness
        //        ViewBag.IsCodeCorrect = false;
        //    }

        //    return View();
        //}










        public void AddStudentToClassroomDB(int classroomCode, string studentName, string studentID, string attendanceStart, string attendanceEnd, string attendanceTime, string status, string studentEmail, string filename, string filepath, string attendanceOption)
        {
            var studentClassroom = new StudentClassroomDB
            {
                Classroom_Code = classroomCode,
                Student_Name = studentName,
                Student_ID = studentID,
                Attendance_Start = attendanceStart,
                Attendance_End = attendanceEnd,
                Attendance_Time = attendanceTime,
                Status = status,
                StudentEmail = studentEmail,
                Filename = filename,
                Filepath = filepath,
                Attendance_Option = attendanceOption
            };

            // Add the studentClassroom to the database
            _context.Student_Clasrooms.Add(studentClassroom);
            _context.SaveChanges();
        }














        public async Task LogUserActivityAsync()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            TimeZoneInfo phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");

            // Convert the UTC time to Philippines Standard Time
            DateTime phTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);

            // Update the Attendance_Time field with the Philippines time
           
            // Log the user's activity to the UserLogs table
            if (user != null)
            {
                var userLog = new Userlogs
                {


                    UniqueId = user.Id,
                    Timestamp = phTime.ToString("yyyy-MM-dd hh:mm:ss tt"),
                    Email = "LoggedIn", // or any other action you want to log
                    UserEmail = user.Email
                };

                _context.Userlogs.Add(userLog);
                await _context.SaveChangesAsync();
            }
        }

  










        public async Task<IActionResult> DataAnalyticsAsync()
        {

            var user = await _userManager.GetUserAsync(User);

            ViewData["UserID"] = _userManager.GetUserId(this.User);




            if (user != null)
            {
                var userAttendanceData = await _context.Student_Clasrooms
                    .Where(sc => sc.StudentEmail == user.UserName)
                    .Select(sc => new
                    {
                        sc.Present,
                        sc.Absent,
                        sc.Late,
                        sc.Excused
                    })
                    .ToListAsync();

                if (userAttendanceData != null && userAttendanceData.Any())
                {
                    var presentCount = userAttendanceData.Sum(sc => sc.Present ?? 0);
                    var absentCount = userAttendanceData.Sum(sc => sc.Absent ?? 0);
                    var lateCount = userAttendanceData.Sum(sc => sc.Late ?? 0);
                    //var excusedCount = userAttendanceData.Sum(sc => sc.Excused ?? 0);
                    // Use ViewBag to pass counts to the view
                    ViewBag.PresentCount = presentCount;
                    ViewBag.AbsentCount = absentCount;
                    ViewBag.LateCount = lateCount;
                    //ViewBag.ExcusedCount = excusedCount;

                    var newDataForTuesday = new AttendanceData
                    {
                        Day = "Monday",
                        Present = presentCount,
                        Absent = absentCount,
                        Late = lateCount
                    };

                    var data = new List<AttendanceData>
                      {
                           new AttendanceData { Day = "Saturday", Absent = 0, Present = 0, Late = 0 },
                           new AttendanceData { Day = "Sunday", Absent = 0, Present = 0, Late = 0 },
                           newDataForTuesday,
                       };

                    ViewBag.AttendanceChartData = data;




                    List<DoughnutChartData> ChartPoints = new List<DoughnutChartData>
                        {
                             new DoughnutChartData { Browser= "Present", Users= ViewBag.PresentCount ?? 0, DataLabelMappingName= $"Present: {ViewBag.PresentCount ?? 0}%" },
                             new DoughnutChartData { Browser= "Absent", Users= ViewBag.AbsentCount ?? 0, DataLabelMappingName= $"Absent: {ViewBag.AbsentCount ?? 0}%" },
                              new DoughnutChartData { Browser= "Late", Users= ViewBag.LateCount ?? 0, DataLabelMappingName= $"Late: {ViewBag.LateCount ?? 0}%" }
                        };

                    ViewBag.ChartPoints = ChartPoints;









                    var userAttendanceData1 = await _context.Student_Clasrooms
              .Where(sc => sc.StudentEmail == user.UserName)
              .Select(sc => new
              {
                  sc.Present,
                  sc.Absent,
                  sc.Late,
                  sc.Classroom_Code
              })
              .ToListAsync();


                    // Calculate total present and absent
                    var totalPresent = userAttendanceData1.Sum(sc => sc.Present ?? 0);
                    var totalAbsent = userAttendanceData1.Sum(sc => sc.Absent ?? 0);
                    var totalLate = userAttendanceData1.Sum(sc => sc.Late ?? 0);

                    // Get the class names for the periods
                    var periods = userAttendanceData1.Select(sc => sc.Classroom_Code).Distinct().ToList();
                    var classNames = await _context.ClassroomDBS
                        .Where(c => periods.Contains(c.ClassCode))
                        .Select(c => new { c.ClassCode, c.ClassName })
                        .ToListAsync();

                    // Map class names to periods
                    var periodClassNames = userAttendanceData1
                        .GroupBy(sc => sc.Classroom_Code)
                        .ToDictionary(g => g.Key, g => classNames.FirstOrDefault(cn => cn.ClassCode == g.Key)?.ClassName);

                    // Prepare the data for the chart
                    var chartData = userAttendanceData1
                        .GroupBy(sc => sc.Classroom_Code)
                        .Select(g => new PolarAreaChartData
                        {
                            Period = (periodClassNames[g.Key] != null ? periodClassNames[g.Key].ToString() : g.Key.ToString()),
                            ProductRevenue_A = g.Sum(sc => sc.Present ?? 0),
                            ProductRevenue_B = g.Sum(sc => sc.Absent ?? 0),
                            ProductRevenue_C = g.Sum(sc => sc.Late ??0)
                            //ProductRevenue_C = 0 // You can add logic here for ProductRevenue_C if needed
                        })
                        .ToList();

                    // Set ViewBag variables for the chart
                    ViewBag.ChartPointsradar = chartData;
                    ViewBag.select = new string[] { "Polar", "Radar" };




                    //List<PolarAreaChartData> ChartPointsradar = new List<PolarAreaChartData>
                    //    {
                    //      new PolarAreaChartData { Period = "2000", ProductRevenue_A = 4  , ProductRevenue_B = 2.6, ProductRevenue_C = 2.8},
                    //      new PolarAreaChartData { Period = "2001", ProductRevenue_A = 3.0, ProductRevenue_B = 2.8, ProductRevenue_C = 2.5 },
                    //      new PolarAreaChartData { Period = "2002", ProductRevenue_A = 3.8, ProductRevenue_B = 2.6, ProductRevenue_C = 2.8 },
                    //      new PolarAreaChartData { Period = "2003", ProductRevenue_A = 3.4, ProductRevenue_B = 3  , ProductRevenue_C = 3.2 },
                    //      new PolarAreaChartData { Period = "2004", ProductRevenue_A = 3.2, ProductRevenue_B = 3.6, ProductRevenue_C = 2.9 },
                    //      new PolarAreaChartData { Period = "2005", ProductRevenue_A = 3.9, ProductRevenue_B = 3  , ProductRevenue_C = 2   }
                    //     };
                    //ViewBag.ChartPointsradar = ChartPointsradar;
                    //ViewBag.select = new string[] { "Polar", "Radar" };






                     totalPresent = userAttendanceData1.Sum(sc => sc.Present ?? 0);
                     totalAbsent = userAttendanceData1.Sum(sc => sc.Absent ?? 0);

                    List<StackedBar100ChartData> ChartPoints8 = new List<StackedBar100ChartData>
                      {
                     new StackedBar100ChartData { Month = "2024", AppleSales = totalPresent, OrangeSales = totalAbsent, Wasteage = 1 },
                        };

                    ViewBag.ChartPoints11 = ChartPoints8;










                    // Calculate the total count of present and absent
                    totalPresent = userAttendanceData1.Sum(sc => sc.Present ?? 0);
                    totalAbsent = userAttendanceData1.Sum(sc => sc.Absent ?? 0);

                    // Calculate the total count of all students
                    int totalStudents = totalPresent + totalAbsent;

                    // Calculate the actual percentages of present and absent
                    int actualPresentPercentage = (int)((double)totalPresent / totalStudents * 100);
                    int actualAbsentPercentage = (int)((double)totalAbsent / totalStudents * 100);

                    // Define the basis percentages
                    double basisPresent = 80;
                    double basisAbsent = 20;

                    // Determine if the actual percentages pass the basis scores
                    bool presentPasses = actualPresentPercentage >= basisPresent;
                    bool absentPasses = actualAbsentPercentage <= basisAbsent;

                    // Set ViewBag variables for the actual percentages and basis scores
                    ViewBag.ActualPresentPercentage = actualPresentPercentage;
                    ViewBag.ActualAbsentPercentage = actualAbsentPercentage;
                    ViewBag.BasisPresent = basisPresent;
                    ViewBag.BasisAbsent = basisAbsent;
                    ViewBag.PresentPasses = presentPasses;
                    ViewBag.AbsentPasses = absentPasses;


















                    // Get the unique classroom codes for the student
                    var classroomCodes = userAttendanceData1.Select(sc => sc.Classroom_Code).Distinct().ToList();

                    // Retrieve the class names for the periods
                    var classNames1 = await _context.ClassroomDBS
                        .Where(c => classroomCodes.Contains(c.ClassCode))
                        .Select(c => new { c.ClassCode, c.ClassName })
                        .ToListAsync();

                    // Group the user attendance data by Classroom_Code
                    var groupedData = userAttendanceData1
                        .GroupBy(sc => sc.Classroom_Code)
                        .Select(g => new
                        {
                            ClassroomCode = g.Key,
                            Present = g.Sum(sc => sc.Present ?? 0),
                            Absent = g.Sum(sc => sc.Absent ?? 0),
                            Late = g.Sum(sc => sc.Late ?? 0)
                        })
                        .ToList();

                    // Prepare the data for the chart
                    var chartData1 = groupedData
                        .Select(g => new BarChartData
                        {
                            Country = classNames.FirstOrDefault(cn => cn.ClassCode == g.ClassroomCode)?.ClassName ?? g.ClassroomCode.ToString(),
                            GDP = g.Present,        
                            WorldShare = g.Absent  ,
                            Late = g.Late
                                                    
                        })
                        .ToList();


                    ViewBag.ChartPointsbar = chartData1;


                    var top3Absent = groupedData.OrderByDescending(g => g.Absent).Take(3).ToList();
                    var top3Present = groupedData.OrderByDescending(g => g.Present).Take(3).ToList();
                    var top3Late = groupedData.OrderByDescending(g => g.Late).Take(3).ToList();


                    ViewBag.Top3Absent = top3Absent;
                    ViewBag.Top3Present = top3Present;
                    ViewBag.Top3Late = top3Late;








                }




                else
                {
                    var data = new List<AttendanceData>
                                      {
                                         new AttendanceData { Day = "Sunday", Absent = 0, Present = 0, Late = 0 },
                                         new AttendanceData { Day = "Monday", Absent = 0, Present = 0, Late = 0 },
                                         new AttendanceData { Day = "Tuesday", Absent = 0, Present = 0, Late = 0 },
                                         new AttendanceData { Day = "Wednesday", Absent = 0, Present = 0, Late = 0 },
                                         new AttendanceData { Day = "Thursday", Absent = 0, Present = 0, Late = 0 },
                                         new AttendanceData { Day = "Friday", Absent = 0, Present = 0, Late = 0 },
                                         new AttendanceData { Day = "Saturday", Absent = 0, Present = 0, Late = 0 },
                                 };

                    ViewBag.AttendanceChartData = data;




                    List<DoughnutChartData> ChartPoints = new List<DoughnutChartData>
                            {
                                new DoughnutChartData { Browser= "Present", Users= 0, DataLabelMappingName= "Present: 0%" },
                                //new DoughnutChartData { Browser= "Late", Users= 0, DataLabelMappingName= "Late: 0%" },
                                new DoughnutChartData { Browser= "Absent", Users= 0, DataLabelMappingName= "Absent: 0%" },
                                //new DoughnutChartData { Browser= "Excuse", Users= 0, DataLabelMappingName= "Excuse: 0%" },
                            };
                    ViewBag.ChartPoints = ChartPoints;





                    //List<PolarAreaChartData> ChartPointsradar = new List<PolarAreaChartData>
                    //{
                    //  new PolarAreaChartData { Period = "2000", ProductRevenue_A = 4  , ProductRevenue_B = 2.6, ProductRevenue_C = 2.8},
                    //  new PolarAreaChartData { Period = "2001", ProductRevenue_A = 3.0, ProductRevenue_B = 2.8, ProductRevenue_C = 2.5 },
                    //  new PolarAreaChartData { Period = "2002", ProductRevenue_A = 3.8, ProductRevenue_B = 2.6, ProductRevenue_C = 2.8 },
                    //  new PolarAreaChartData { Period = "2003", ProductRevenue_A = 3.4, ProductRevenue_B = 3  , ProductRevenue_C = 3.2 },
                    //  new PolarAreaChartData { Period = "2004", ProductRevenue_A = 3.2, ProductRevenue_B = 3.6, ProductRevenue_C = 2.9 },
                    //  new PolarAreaChartData { Period = "2005", ProductRevenue_A = 3.9, ProductRevenue_B = 3  , ProductRevenue_C = 2   }
                    // };
                    //ViewBag.ChartPointsradar = ChartPointsradar;
                    //ViewBag.select = new string[] { "Polar", "Radar" };





                    List<PolarAreaChartData> ChartPointsradar = new List<PolarAreaChartData>
                        {
                          new PolarAreaChartData { Period = "Classroom A", ProductRevenue_A = 0  , ProductRevenue_B = 0, ProductRevenue_C = 0},
                          new PolarAreaChartData { Period = "Classroom B", ProductRevenue_A = 0, ProductRevenue_B = 0, ProductRevenue_C = 0 },


                         };
                    ViewBag.ChartPointsradar = ChartPointsradar;
                    ViewBag.select = new string[] { "Polar", "Radar" };




                    List<StackedBar100ChartData> ChartPoints8 = new List<StackedBar100ChartData>
                     {
                       new StackedBar100ChartData { Month = "2024", AppleSales = 0, OrangeSales = 0, Wasteage = 1 },

                      };
                    ViewBag.ChartPoints11 = ChartPoints8;




                    List<BarChartData> ChartPoints1 = new List<BarChartData>
            {
                new BarChartData { Country = "Canada",  GDP = 3.05 , WorldShare = 2.04 },
                new BarChartData { Country = "Italy", GDP = 1.50 , WorldShare = 2.40 },
                new BarChartData { Country = "Germany",  GDP = 2.22, WorldShare = 4.56 },
                new BarChartData { Country = "India", GDP = 6.68 , WorldShare = 3.28  },
                new BarChartData { Country = "France",  GDP = 1.82, WorldShare = 3.19 },
                new BarChartData { Country = "Japan",  GDP = 1.71, WorldShare = 6.02 }
            };
                    ViewBag.ChartPoints = ChartPoints1;




                }





            }




            return View();
        }


    public class BarChartData
    {
        public string Country;
        public double GDP;
        public double WorldShare;
        public double Late;
    }
    public class PolarAreaChartData
        {
            public string Period;
            public double ProductRevenue_A;
            public double ProductRevenue_B;
            public double ProductRevenue_C;
        }



        public class StackedBar100ChartData
        {
            public string Month;
            public double AppleSales;
            public double OrangeSales;
            public double Wasteage;
        }











        //      public async Task<IActionResult> Index()
        //    {


        //        var user = await _userManager.GetUserAsync(User);


        //        ViewData["UserID"]=_userManager.GetUserId(this.User);



        //        List<DoughnutChartData> ChartPoints = new List<DoughnutChartData>
        //        {
        //            new DoughnutChartData { Browser= "Present", Users= 40.3, DataLabelMappingName= "Present: 40.3%" },
        //            new DoughnutChartData { Browser= "Late", Users= 10.6, DataLabelMappingName= "Late: 10.6%" },
        //            new DoughnutChartData { Browser= "Absent", Users= 15.0, DataLabelMappingName= "Absent: 15.0%" },
        //            new DoughnutChartData { Browser= "Excuse", Users= 5.7, DataLabelMappingName= "Excuse: 5.7%" },
        //        };
        //        ViewBag.ChartPoints = ChartPoints;


        //        var data = new List<AttendanceData>
        //{
        //    new AttendanceData { Day = "Monday", Absent = 0, Present = 0, Late = 0 },
        //    new AttendanceData { Day = "Tuesday", Absent = 10, Present = 11, Late = 23 },
        //    new AttendanceData { Day = "Wednesday", Absent = 20, Present = 45, Late = 30 },
        //    // Add more data for other days
        //};

        //        ViewBag.AttendanceChartData = data;

        //        return View();
        //    }












        public class DoughnutChartData
        {
            public string Browser;
            public double Users;
            public string DataLabelMappingName;

        }

        public class AttendanceData
        {
            public string Day { get; set; }
            public int Absent { get; set; }
            public int Present { get; set; }
            public int Late { get; set; }
        }








        //public IActionResult ProcessClassCode(string classCode)
        //{
        //    // Process the class code here (e.g., validate, save to database, etc.)
        //    if (!string.IsNullOrEmpty(classCode))
        //    {
        //        // Process the class code
        //        return RedirectToAction("Index");
        //    }

        //    // If the class code is invalid or empty, return to the form
        //    return RedirectToAction("Index");
        //}



        //public async Task<IActionResult> CheckCodeAsync(int code)
        //{
        //    var classCode = _context.ClassroomDBS.FirstOrDefault(c => c.ClassCode == code);

        //    var user = await _userManager.GetUserAsync(User);


        //    if (classCode != null && user is Student student)
        //    {

        //        string studentName = student.FirstName + student.LastName; // Replace with the actual property name in your IdentityUser model
        //        string studentID = student.IDnumber; // Replace with the actual property name in your IdentityUser model
        //        string studentEmail = student.EmailStudent;
        //        string filename = student.FileName;
        //        string filepath = student.FilePath;

        //        // Check if the user is already a member of this class
        //        var isUserAlreadyAMember = await _context.Student_Clasrooms
        //            .AnyAsync(sc => sc.Student_ID == studentID && sc.Classroom_Code == classCode.ClassCode);

        //        if (!isUserAlreadyAMember)
        //        {
        //            var classroomRecord = await _context.ClassroomDBS.FirstOrDefaultAsync(c => c.ClassCode == code);

        //            if (classroomRecord != null)
        //            {
        //                string AttendanceStart = classroomRecord.Attendance_Start;
        //                string AttendancEnd = classroomRecord.Attendance_End;
        //                string AttendanceOption = classroomRecord.Attendance_Option;

        //                AddStudentToClassroomDB(code, studentName, studentID, AttendanceStart, AttendancEnd, "Pending Time", "Accept", studentEmail, filename, filepath, AttendanceOption);

        //                // Set a ViewBag to indicate correctness
        //                ViewBag.IsCodeCorrect = true;

        //                return RedirectToAction("Index");
        //            }
        //        }
        //        else
        //        {
        //            // User is already a member, show an error message
        //            ViewBag.IsCodeCorrect = false;
        //            ViewBag.ErrorMessage = "You are already a member of this class.";

        //        }
        //    }
        //    else
        //    {
        //        // Code is incorrect, set a ViewBag to indicate incorrectness
        //        ViewBag.IsCodeCorrect = false;

        //    }
        //    return View();

        //}





        //public async Task<IActionResult> CheckCodeAsync(int code)
        //{
        //    //ViewBag.IsCodeCorrect = null;

        //    var classCode = _context.ClassroomDBS.FirstOrDefault(c => c.ClassCode == code);
        //    var classCodes = await _context.ClassroomDBS.ToListAsync();
        //    ViewBag.ClassCodes = classCodes;

           
            

        //    if (classCode != null /*&& user is Student student*/)
        //    {

        //        var user = await _userManager.GetUserAsync(User);


        //        var student = await _context.Students.FirstOrDefaultAsync(s => s.EmailStudent == user.UserName);


        //        string studentName = student.FirstName + student.LastName; // Replace with the actual property name in your IdentityUser model
        //        string studentID = student.IDnumber; // Replace with the actual property name in your IdentityUser model
        //        string studentEmail = student.EmailStudent;
        //        string filename = student.FileName;
        //        string filepath = student.FilePath;

        //        // Check if the user is already a member of this class
        //        var isUserAlreadyAMember = await _context.Student_Clasrooms
        //            .AnyAsync(sc => sc.Student_ID == studentID && sc.Classroom_Code == classCode.ClassCode);

        //        if (!isUserAlreadyAMember)
        //        {
        //            var classroomRecord = await _context.ClassroomDBS.FirstOrDefaultAsync(c => c.ClassCode == code);

        //            //if (classroomRecord != null)
        //            //{
        //                string AttendanceStart = classroomRecord.Attendance_Start;
        //                string AttendancEnd = classroomRecord.Attendance_End;
        //                string AttendanceOption = classroomRecord.Attendance_Option;

        //                AddStudentToClassroomDB(code, studentName, studentID, AttendanceStart, AttendancEnd, "Pending Time", "Accept", studentEmail, filename, filepath, AttendanceOption);

        //                // Set a ViewBag to indicate correctness
        //                //ViewBag.IsCodeCorrect = true;

        //                return RedirectToAction("Index");
        //            //}
        //        }
        //        else
        //        {
        //            // User is already a member, show an error message
        //            ViewBag.IsCodeCorrect = false;
        //            ViewBag.ErrorMessage = "You are already a member of this class.";

        //        }
        //    }
        //    else
        //    {
        //        // Code is incorrect, set a ViewBag to indicate incorrectness
        //        ViewBag.IsCodeCorrect = false;

        //    }

        //    //return View("CheckCode", _context.ClassroomDBS.ToList());
        //    return View();
        //}







        //private void AddStudentToClassroomDB(int classroomCode, string studentName, string studentID, string attendanceStart, string attendanceEnd, string attendanceTime, string status, string studentEmail, string filename, string filepath, string attendanceOption)
        //{
        //    var studentClassroom = new StudentClassroomDB
        //    {
        //        Classroom_Code = classroomCode,
        //        Student_Name = studentName,
        //        Student_ID = studentID,
        //        Attendance_Start = attendanceStart,
        //        Attendance_End = attendanceEnd,
        //        Attendance_Time = attendanceTime,
        //        Status = status,
        //        StudentEmail = studentEmail,
        //        Filename = filename,
        //        Filepath = filepath,
        //        Attendance_Option = attendanceOption
        //    };

        //    // Add the studentClassroom to the database
        //    _context.Student_Clasrooms.Add(studentClassroom);
        //    _context.SaveChanges();
        //}














        public IActionResult StudentClassroom()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}