using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APYROPROJECTFINAL.Data;
using APYROPROJECTFINAL.Models;
using APYROPROJECTFINAL.Contants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using APYROPROJECTFINAL.Areas.Identity.Data;
using Newtonsoft.Json;
using System.Text.Json;
using Syncfusion.EJ2.FileManager.Base;




using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Http.Extensions;
using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.Extensions.Hosting;

namespace APYROPROJECTFINAL.Controllers
{

    [Authorize(Roles = "Educator")]
    public class ClassroomDBsController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<ClassroomDBsController> _logger;

        public ClassroomDBsController(ILogger<ClassroomDBsController> logger, IWebHostEnvironment hostingEnvironment, AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            this._userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        //public async Task<IActionResult> Index()
        //{
        //    return _context.ClassroomDBS != null ?
        //                View(await _context.ClassroomDBS.ToListAsync()) :
        //                Problem("Entity set 'AuthDBContext.ClassroomDBS'  is null.");
        //}



        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var educator = await _context.Educators.FirstOrDefaultAsync(m => m.EmailEducator == user.UserName);

            if (educator != null)
            {
                var classroomData = await _context.ClassroomDBS
                    .Where(c => c.EducatorEmail == user.UserName)
                    .ToListAsync();

                return View(classroomData);
            }

            return Problem("User is not an educator.");
        }





        public async Task<ActionResult> CreatenewAsync(string SaveOption, string valueINeed)
        {



            if (SaveOption == null)
                return View();



            int classIDParse = int.Parse(valueINeed);

            var classroomDB = await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.ClassID == classIDParse);





            // ViewBag.StudentNames = studentClassroomsDB.Select(s => s.Student_Name).ToList();

            //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
            //The instantiation process consists of two steps.

            //Step 1 : Instantiate the spreadsheet creation engine.
            ExcelEngine excelEngine = new ExcelEngine();
            //Step 2 : Instantiate the excel application object.
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(3);
            IWorksheet sheet = workbook.Worksheets[0];


            #region Generate Excel
            sheet.Range["A2"].ColumnWidth = 30;
            sheet.Range["B2"].ColumnWidth = 30;
            sheet.Range["C2"].ColumnWidth = 30;
            sheet.Range["D2"].ColumnWidth = 30;
            sheet.Range["E2"].ColumnWidth = 30;
            sheet.Range["F2"].ColumnWidth = 30;

            sheet.Range["A2:D2"].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A2"].Text = "APYRO ATTENDANCE";
            sheet.Range["A2"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A2"].CellStyle.Font.Bold = true;
            sheet.Range["A2"].CellStyle.Font.Size = 28;
            sheet.Range["A2"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A2"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet.Range["A7"].Text = "Educator";
            if (classroomDB != null)
            {
                sheet.Range["B7"].Text = classroomDB.Educator_Name;

                //sheet.Range["B4"].Text = "Roger Federer";
                sheet.Range["A3:B7"].CellStyle.Font.FontName = "Verdana";
                sheet.Range["A3:B7"].CellStyle.Font.Bold = true;
                sheet.Range["A3:B7"].CellStyle.Font.Size = 11;
                sheet.Range["A3:A7"].CellStyle.Font.RGBColor = Color.FromArgb(0, 128, 128, 128);
                sheet.Range["A3:A7"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
                sheet.Range["B3:B7"].CellStyle.Font.RGBColor = Color.FromArgb(0, 174, 170, 170);
                sheet.Range["B3:B7"].HorizontalAlignment = ExcelHAlign.HAlignRight;



                sheet.Range["A9:D20"].CellStyle.Font.FontName = "Verdana";
                sheet.Range["A9:D20"].CellStyle.Font.Size = 11;


                var user = await _userManager.GetUserAsync(User);

                if(user!=null)
                {
                    var University = await _context.Educators.FirstOrDefaultAsync(m => m.EmailEducator == user.UserName);


                    if(University!=null)
                    {
                        sheet.Range["A3"].Text = "University";
                        sheet.Range["B3"].Text = University.University;
                    }

        
                }



                sheet.Range["A4"].Text = "Department";
                sheet.Range["B4"].Text = "CCIS";


                sheet.Range["A6"].Text = "Section";
                sheet.Range["B6"].Text = classroomDB.Section;
                //sheet.Range["B6"].Text = "IV-ACSSC";
                //sheet.Range["B6"].NumberFormat = "m/d/yyyy";
                //sheet.Range["B6"].DateTime = DateTime.Parse("10/20/2012", CultureInfo.InvariantCulture);

                sheet.Range["A5"].Text = "Subject";
                sheet.Range["B5"].Text = classroomDB.ClassName;
                //sheet.Range["B7"].Text = "Calculus";
            }

         
            var currentUrl = HttpContext.Request.GetDisplayUrl();
            var lastSegment = Path.GetFileName(new Uri(currentUrl).AbsolutePath);

            //int classIDParse = int.Parse(lastSegment);

            _logger.LogInformation($"Last Segment: {valueINeed}");


         

            if (classroomDB != null)
            {
                var studentData = await _context.Student_Clasrooms
                .Where(s => s.Classroom_Code == classroomDB.ClassCode)
                .Select(s => new { s.Student_Name, s.Student_ID, s.Attendance_Start, s.Attendance_End, s.Attendance_Time, s.Status })
                    .ToListAsync();

                if (studentData.Any())
                {
                    int cellIndex = 10; // Starting cell index for names and IDs (e.g., "A10" for names and "B10" for IDs)

                    foreach (var data in studentData)
                    {
                        // Check if the status is not "Accept" before adding to the sheet
                        if (data.Status != "Accept")
                        {
                            sheet.Range["A" + cellIndex].Text = data.Student_Name;
                            sheet.Range["B" + cellIndex].Text = data.Student_ID;
                            sheet.Range["C" + cellIndex].Text = data.Attendance_Start;
                            sheet.Range["D" + cellIndex].Text = data.Attendance_End;
                            sheet.Range["E" + cellIndex].Text = data.Attendance_Time;
                            sheet.Range["F" + cellIndex].Text = data.Status;
                            cellIndex++;
                        }
                    }

                    //foreach (var data in studentData)//just remove if statement , if wrong logic
                    //{
                    //    sheet.Range["A" + cellIndex].Text = data.Student_Name;
                    //    sheet.Range["B" + cellIndex].Text = data.Student_ID;
                    //    sheet.Range["C" + cellIndex].Text = data.Attendance_Start;
                    //    sheet.Range["D" + cellIndex].Text = data.Attendance_End;
                    //    sheet.Range["E" + cellIndex].Text = data.Attendance_Time;
                    //    sheet.Range["F" + cellIndex].Text = data.Status;
                    //    cellIndex++;
                    //}
                }

            }




            sheet.Range["A20:F20"].CellStyle.Color = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A20:F20"].CellStyle.Font.Color = ExcelKnownColors.White;
            sheet.Range["A20:F20"].CellStyle.Font.Bold = true;

            IStyle style = sheet["B9:F9"].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignRight;
            style.Color = Color.FromArgb(0, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.White;

            sheet.Range["A9"].Text = "Name";
            sheet.Range["A9"].CellStyle.Color = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A9"].CellStyle.Font.Color = ExcelKnownColors.White;
            sheet.Range["A9"].CellStyle.Font.Bold = true;
            sheet.Range["B9"].Text = "Student ID";

      

            sheet.Range["C9"].Text = "Attendance Start";
    

            sheet.Range["D9"].Text = "Attendance End";
    
            #endregion




            sheet.Range["E9"].Text = "Attendance Time";



            sheet.Range["F9"].Text = "Status";
     

            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "AttendanceApyro.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "AttendanceApyro.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }


        private string ResolveApplicationPath(string fileName)
        {
            return _hostingEnvironment.WebRootPath + "//XlsIO//" + fileName;
        }
        private string ResolveApplicationImagePath(string fileName)
        {
            return _hostingEnvironment.WebRootPath + "//Images//XlsIO//" + fileName;
        }































        // GET: ClassroomDBs/Details/5

        public int classCodes;
        // public int classid;

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




            var studentClassroomsDB = await _context.Student_Clasrooms
                .Where(s => s.Classroom_Code == classroomDB.ClassCode)
                .ToListAsync();

            var viewModel = new ClassroomViewModel
            {
                ClassroomDB = classroomDB,
                Students = studentClassroomsDB
            };




            //var classCode = classroomDB.ClassCode;

            //ViewBag.ClassCode = classCode; // Store the classCode in ViewBag

            //await GetClassroomData(); // Call the method without a parameter


            classCodes = classroomDB.ClassCode;

            ViewBag.Classid = id;


            //ViewBag.loading = "yes";


            var currentUrl = HttpContext.Request.GetDisplayUrl();
            var lastSegment = Path.GetFileName(new Uri(currentUrl).AbsolutePath);


            ViewBag.Class = lastSegment;



            // classCode = classroomDB.ClassCode;


            //classid = classroomDB.ClassID;

            //  classCodes = classroomDB.ClassCode;

            //await GetClassroomData();

            //ViewBag.Class = classCode;
            //ViewBag.Classid = classid;

            // Call the method without a parameter

            //classCode = classroomDB.ClassCode;








            var user = await _userManager.GetUserAsync(User);



            var classroomDB1 = await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.ClassID == id);


            if (classroomDB1 != null)
            {
                var educator = await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.EducatorEmail == user.UserName);
                if (educator != null)
                {

                    if (classroomDB.TrackerStatus == "ON")
                    {
                        ViewBag.enable = "Disable Tracker";
                    }
                    else
                    {
                        ViewBag.enable = "Enable Tracker";
                    }

                }

            } 




                    return View(viewModel);
        }





        //public async Task<JsonResult> GetClassroomData()
        //{
        //    //  ViewBag.Id = classCode;
        //    //   var classCode = (int)ViewBag.ClassCode;


        //    var user = await _userManager.GetUserAsync(User);

        //    if (user != null)
        //    {

        //        var data = _context.Student_Clasrooms
        //        .Where(s => s.Classroom_Code == classCode)
        //        .Select(s => new
        //        {
        //            ClassCode = s.Classroom_Code,
        //            StudentName = s.Student_Name,
        //            ImageFile = s.Filename,

        //        })
        //        .ToList();

        //        ViewBag.ClassData = data;
        //        return Json(data);
        //    }
        //    return Json(null);
        //}




        //public async Task<JsonResult> GetClassroomData()
        //{

        //    var classroomDB = await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.ClassID == classid);
        //    if (classroomDB == null)
        //    {
        //        return Json(null);
        //    }

        //    var classroomCode = classroomDB.ClassCode;

        //    var students = await _context.Student_Clasrooms
        //        .Where(s => s.Classroom_Code == classroomCode)
        //        .Select(s => new
        //        {
        //            ClassCode = s.Classroom_Code,
        //            StudentName = s.Student_Name,
        //            ImageFile = s.Filename
        //        })
        //        .ToListAsync();

        //    ViewBag.Class = classCode;
        //    ViewBag.Classid = classid;
        //    return Json(students);
        //}





        public async Task<JsonResult> GetClassroomData()
        {
            // Retrieve the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Find all the classroom codes managed by the educator
                var classroomCodes = _context.ClassroomDBS
                    .Where(e => e.EducatorEmail == user.UserName)
                    .Select(e => e.ClassCode)
                    .ToList();

                // Retrieve class codes, student names, and image file names based on the educator's classroom codes
                var data = _context.Student_Clasrooms
                    .Where(s => classroomCodes.Contains(s.Classroom_Code))
                    .Select(s => new
                    {
                        ClassCode = s.Classroom_Code,
                        StudentName = s.Student_Name,
                        ImageFile = s.Filename,
                    })
                    .ToList();

                return Json(data);
            }

            return Json(null); // Handle cases where user is not found
        }




        public IActionResult videoattendance()
        {
            return View();
        }

























        //public async Task<IActionResult> DisplayStudentData()
        //{
        //    var students = await _context.Student_Clasrooms
        //        .Where(s => s.Classroom_Code == classCode)
        //        .ToListAsync();

        //    return View(students);
        //}



















        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.ClassroomDBS == null)
        //    {
        //        return NotFound();
        //    }

        //    var classroomDB = await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.ClassID == id);

        //    if (classroomDB == null)
        //    {
        //        return NotFound();
        //    }

        //    var studentClassroomsDB = await _context.Student_Clasrooms
        //        .Where(s => s.Classroom_Code == classroomDB.ClassCode)
        //        .ToListAsync();

        //    var viewModel = new ClassroomViewModel
        //    {
        //        ClassroomDB = classroomDB,
        //        Students = studentClassroomsDB
        //    };

        //    return View(viewModel);
        //}





        public async Task<IActionResult> ProcessStudentDataAsync([FromBody] FetchStudent fetchStudent)
        {
            var studentName = fetchStudent.studentName;
            var classCode = fetchStudent.classCode;
            var classidss = fetchStudent.classId;

            var classroomDB = await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.ClassID == classidss);

            ViewBag.Classids = classidss;


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
                student.Status = "Present";


                TimeZoneInfo phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");

                // Convert the UTC time to Philippines Standard Time
                DateTime phTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);

                // Update the Attendance_Time field with the Philippines time
                student.Attendance_Time = phTime.ToString("yyyy-MM-dd hh:mm:ss tt");




                //student.Attendance_Time = DateTime.Now.ToString();

                student.Present = (student.Present ?? 0) + 1;
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











        //public async Task<IActionResult> ProcessStudentDataAsync([FromBody] FetchStudent fetchStudent)
        //{
        //    var studentName = fetchStudent.studentName;
        //    var classCode = fetchStudent.classCode;
        //    var classidss = fetchStudent.classId;

        //    var classroomDB = await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.ClassID == classidss);

        //    ViewBag.Classids = classidss;


        //    if (classroomDB == null)
        //    {
        //        return NotFound();
        //    }
        //    var classcoding = classroomDB.ClassCode;




        //    // Retrieve the student based on provided studentName and classCode
        //    var student = await _context.Student_Clasrooms
        // .FirstOrDefaultAsync(s => s.Student_Name == studentName && s.Classroom_Code == classCode && s.Classroom_Code == classcoding);


        //    if (student != null)
        //    {
        //        // Update the Status field to "Present"
        //        student.Status = "Present";

        //        // Save changes to the database
        //        await _context.SaveChangesAsync();

        //        var responseData = new { Message = "Data received successfully. Student: " + studentName + ", Class Code: " + classCode + ". Status updated to 'Present'" };
        //        return Json(responseData);
        //    }
        //    else
        //    {
        //        var responseData = new { Message = "Student data not found or does not match." };
        //        return Json(responseData);
        //    }
        //}
















        //public async Task<IActionResult> ProcessStudentDataAsync([FromBody] FetchStudent fetchStudent)
        //{
        //    var studentName = fetchStudent.studentName;
        //    var classCode = fetchStudent.classCode;

        //    // Retrieve the student based on provided studentName and classCode
        //    var student = await _context.Student_Clasrooms
        //        .FirstOrDefaultAsync(s => s.Student_Name == studentName && s.Classroom_Code == classCode);

        //    if (student != null)
        //    {
        //        // Update the Status field to "Present"
        //        student.Status = "Present";

        //        // Save changes to the database
        //        await _context.SaveChangesAsync();

        //        var responseData = new { Message = "Data received successfully. Student: " + studentName + ", Class Code: " + classCode + ". Status updated to 'Present'" };
        //        return Json(responseData);
        //    }
        //    else
        //    {
        //        var responseData = new { Message = "Student data not found or does not match." };
        //        return Json(responseData);
        //    }
        //}














        //public async Task<IActionResult> ProcessStudentDataAsync([FromBody] FetchStudent fetchStudent)
        //{

        //    var studentname = fetchStudent.studentName;
        //    var classcode = fetchStudent.classCode;


        //    var responseData = new { Message = "Data received successfully. Student: " + studentname + ", Class Code: " + classcode };
        //    return Json(responseData);
        //}


































        //public async Task<JsonResult> GetClassroomData()
        //{
        //    // Retrieve the currently logged-in user
        //    var user = await _userManager.GetUserAsync(User);

        //    if (user != null)
        //    {
        //        // Find the classroomDB with the matching username
        //        var classroomDB = _context.ClassroomDBS.FirstOrDefault(e => e.EducatorEmail == user.UserName);

        //        if (classroomDB != null)
        //        {
        //            var classroomCode = classroomDB.ClassCode;

        //            // Retrieve class codes, student names, and image file names based on the classroom code
        //            var data = _context.Student_Clasrooms
        //                .Where(s => s.Classroom_Code == classroomCode)
        //                .Select(s => new
        //                {
        //                    ClassCode = s.Classroom_Code,
        //                    StudentName = s.Student_Name,
        //                    ImageFile = s.Filename,
        //                })
        //                .ToList();


        //            return Json(data);
        //        }
        //    }

        //    return Json(null); // Handle cases where user or classroomDB is not found
        //}



        //----










        //public async Task<JsonResult> GetClassroomData()
        //{
        //    // Retrieve the currently logged-in user
        //    var user = await _userManager.GetUserAsync(User);

        //    if (user != null)
        //    {
        //        // Find all the classroom codes managed by the educator
        //        var classroomCodes = _context.ClassroomDBS
        //            .Where(e => e.EducatorEmail == user.UserName)
        //            .Select(e => e.ClassCode)
        //            .ToList();

        //        // Retrieve class codes, student names, and image file names based on the educator's classroom codes
        //        var data = _context.Student_Clasrooms
        //            .Where(s => classroomCodes.Contains(s.Classroom_Code))
        //            .Select(s => new
        //            {
        //                ClassCode = s.Classroom_Code,
        //                StudentName = s.Student_Name,
        //                ImageFile = s.Filename,
        //            })
        //            .ToList();

        //        return Json(data);
        //    }

        //    return Json(null); // Handle cases where user is not found
        //}












        //public async Task<JsonResult> GetClassroomData()
        //{
        //    // Retrieve the currently logged-in user
        //    var user = await _userManager.GetUserAsync(User);

        //    if (user != null)
        //    {
        //        // Find the classroomDB with the matching username
        //        var classroomDB = _context.ClassroomDBS.FirstOrDefault(e => e.EducatorEmail == user.UserName);

        //        if (classroomDB != null)
        //        {
        //            var classroomCode = classroomDB.ClassCode;

        //            // Retrieve class codes, student names, and image file names based on the classroom code
        //            var data = _context.Student_Clasrooms
        //                .Where(s => s.Classroom_Code == classroomCode)
        //                .Select(s => new
        //                {
        //                     ClassCode = s.Classroom_Code,
        //                     StudentName = s.Student_Name,
        //                     ImageFile = s.Filename,
        //                })
        //                .ToList();


        //            return Json(data);
        //        }
        //    }

        //    return Json(null); // Handle cases where user or classroomDB is not found
        //}






        //public async Task<JsonResult> GetClassroomData()
        //{
        //    // Retrieve the currently logged-in user
        //    var user = await _userManager.GetUserAsync(User);

        //    if (user != null)
        //    {
        //        // Find the classroomDB with the matching username
        //        var classroomDB = _context.ClassroomDBS.FirstOrDefault(e => e.EducatorEmail == user.UserName);

        //        if (classroomDB != null)
        //        {
        //            var classroomCode = classroomDB.ClassCode;

        //            // Retrieve class codes, student names, and image file names based on the classroom code
        //            var data = _context.Student_Clasrooms
        //                .Where(s => s.Classroom_Code == classroomCode)
        //                .Select(s => new
        //                {
        //                    ClassCode = s.Classroom_Code,
        //                    StudentName = s.Student_Name,
        //                    ImageFile = s.Filename,
        //                })
        //                .ToList();


        //            return Json(data);
        //        }
        //    }

        //    return Json(null); // Handle cases where user or classroomDB is not found
        //}











        //public JsonResult GetImage()
        //{
        //    string imageUrl = "/booking.png"; // Replace with the actual image URL
        //    return Json(new { imageUrl });
        //}

        //public JsonResult GetStringData()
        //{
        //    string message = "Hello, World!";
        //    return Json(new { message });
        //}












        //public async Task<JsonResult> GetClassroomData()
        //{
        //    // Retrieve the currently logged-in user
        //    var user = await _userManager.GetUserAsync(User);

        //    if (user != null)
        //    {
        //        // Find the classroomDB with the matching username
        //        var classroomDB = _context.ClassroomDBS.FirstOrDefault(e => e.EducatorEmail == user.UserName);

        //        if (classroomDB != null)
        //        {
        //            var classroomCode = classroomDB.ClassCode;

        //            // Retrieve class codes, student names, and image file names based on the classroom code
        //            var data = _context.Student_Clasrooms
        //                .Where(s => s.Classroom_Code == classroomCode)
        //                .Select(s => new
        //                {
        //                    ClassCode = s.Classroom_Code,
        //                    StudentName = s.Student_Name,
        //                    ImageFile = s.Filename

        //                })
        //                .ToList();

        //            var json = JsonConvert.SerializeObject(data);

        //            return Json(json, new Newtonsoft.Json.JsonSerializerSettings());
        //        }
        //    }

        //    return Json(null); // Handle cases where user or classroomDB is not found
        //}













        //public async Task<JsonResult> GetClassroomData()
        //{
        //    // Retrieve the currently logged-in user
        //    var user = await _userManager.GetUserAsync(User);

        //    if (user != null)
        //    {
        //        // Find the classroomDB with the matching username
        //        var classroomDB = _context.ClassroomDBS.FirstOrDefault(e => e.EducatorEmail == user.UserName);

        //        if (classroomDB != null)
        //        {
        //            var classroomCode = classroomDB.ClassCode;

        //            // Retrieve class codes, student names, and image file names based on the classroom code
        //            var data = _context.Student_Clasrooms
        //                .Where(s => s.Classroom_Code == classroomCode)
        //                .Select(s => new
        //                {
        //                    ClassCode = s.Classroom_Code,
        //                    StudentName = s.Student_Name,
        //                    ImageFile = ConvertFileToBase64(s.Filename)
        //                })
        //                .ToList();

        //            return Json(data);
        //        }
        //    }

        //    return Json(null); // Handle cases where user or classroomDB is not found
        //}


        //private string ConvertFileToBase64(string filePath)
        //{
        //    if (System.IO.File.Exists(filePath))
        //    {
        //        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        //        return Convert.ToBase64String(fileBytes);
        //    }

        //    return string.Empty; // Handle cases where the file does not exist or cannot be read
        //}













        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.ClassroomDBS == null)
        //    {
        //        return NotFound();
        //    }

        //    var classroomDB = await _context.ClassroomDBS
        //        .FirstOrDefaultAsync(m => m.ClassID == id);

        //    if (classroomDB == null)
        //    {
        //        return NotFound();
        //    }

        //    var classroomViewModel = new ClassroomViewModel
        //    {
        //        ClassroomDB = classroomDB
        //    };

        //    return View(classroomViewModel);
        //}











        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.ClassroomDBS == null)
        //    {
        //        return NotFound();
        //    }

        //    var classroomDB = await _context.ClassroomDBS
        //        .FirstOrDefaultAsync(m => m.ClassID == id);
        //    if (classroomDB == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(classroomDB);
        //}





        // GET: ClassroomDBs/Create

        public IActionResult Create()
        {

            return View();
        }



        // POST: ClassroomDBs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassID,Attendance_Option,ClassCode,Educator_Name,ClassName,Section,Attendance_Start,Attendance_End,Days,EducatorEmail")] ClassroomDB classroomDB)
        {
           
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var UserName = user.UserName;
                var educator = _context.Educators.FirstOrDefault(e => e.EmailEducator == UserName);
                if (educator != null)
                {
                    classroomDB.Educator_Name = educator.FirstName + " " + educator.LastName;
                    classroomDB.EducatorEmail = UserName;
                    Random random = new Random();

                    // Generate a random 6-digit number
                    int classCode = random.Next(100000, 999999);

                    classroomDB.ClassCode = classCode;
                    classroomDB.Attendance_Option = "Group Recognition";

                    _context.Add(classroomDB);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Educator");
                }
            }

            return View(classroomDB);
        }







        //[HttpPost]
        //public IActionResult Tracker(string trackerstatus , string classcode1)
        //{
        //    Debug.WriteLine($"Tracker Status: {trackerstatus}");

        //    if (trackerstatus == "ON")
        //    {
        //        // Logic for when trackerstatus is "ON
        //        Debug.WriteLine($"Tracker Status: {trackerstatus}");
        //        Debug.WriteLine($"Tracker classcode: {classcode1}");
        //        return Ok(new { success = true, message = "Tracker status is ON" });

        //    }
        //    else
        //    {
        //        Debug.WriteLine($"Tracker Status: {trackerstatus}");
        //        // Logic for when trackerstatus is not "ON"
        //        return Json(new { success = false, message = "Tracker status is not ON" });
        //    }
        //}




        //[HttpPost]
        //public async Task<IActionResult> TrackerAsync(string trackerstatus, string classcode1)
        //{

        //    int classIDParse = int.Parse(classcode1);

        //    var classroomDB = await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.ClassID == classIDParse);

        //    if(classroomDB != null)
        //    {
        //         var studenttracker = _context.Student_Clasrooms
        //        .FirstOrDefault(sc => sc.Classroom_Code == classroomDB.ClassCode);


        //        if (studenttracker != null)
        //        {
        //         studenttracker.Tracker = trackerstatus;
        //        _context.SaveChanges();
        //        }


        //        return Json(new { success = true, message = "Student Tracker Updated" });
        //    }





        //    return Json(new { success = false, message = "Student Tracker not updated" });
        //}





        [HttpPost]
        public async Task<IActionResult> TrackerAsync(string trackerstatus, string classcode1)
        {
            int classIDParse = int.Parse(classcode1);

            var classroomDB = await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.ClassID == classIDParse);

            if (classroomDB != null)
            {
                var studenttrackers = _context.Student_Clasrooms
                    .Where(sc => sc.Classroom_Code == classroomDB.ClassCode)
                    .ToList();

                foreach (var studenttracker in studenttrackers)
                {

                    if (studenttracker.Tracker == "ON")
                    {
                        studenttracker.Tracker = "OFF";
                      
                    }
                    else 
                    {
                        studenttracker.Tracker = "ON";
                       
                    }

                }

                _context.SaveChanges();

                return Json(new { success = true, message = "Student Trackers Updated" });
            }

            return Json(new { success = false, message = "No Student Trackers Updated" });
        }



        [HttpPost]
        public async Task<IActionResult> TrackerEducatorAsync(string trackereducator ,string classcode2)
        {
            var user = await _userManager.GetUserAsync(User);

            int classIDParse = int.Parse(classcode2);

            var classroomDB = await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.ClassID == classIDParse);


            if (classroomDB != null)
            {
                var educator =  await _context.ClassroomDBS.FirstOrDefaultAsync(m => m.EducatorEmail == user.UserName);
                if (educator != null)
                {

                    if (classroomDB.TrackerStatus == "ON")
                    {
                        classroomDB.TrackerStatus = "OFF";
                    }
                    else
                    {
                        classroomDB.TrackerStatus = "ON";
                    }

                    //classroomDB.TrackerStatus = trackereducator;

                    _context.SaveChanges();

                    return Json(new { success = true, message = "Educator Trackers Updated" });
                }


            }
            return Json(new { success = false, message = "No Educator Trackers Updated" });

        }





















        [HttpPost]
        public IActionResult UpdateStatus(string studentId, string classcode)
        {
            // Convert studentId to int if it's an integer
           

            var studentClassroom = _context.Student_Clasrooms
                .FirstOrDefault(sc => sc.Classroom_Code.ToString() == classcode && sc.Student_ID == studentId);

            if (studentClassroom != null)
            {
                studentClassroom.Status = "Pending";
                _context.SaveChanges();

                return Json(new { success = true, message = "Status updated successfully to Pending" });
            }

            return Json(new { success = false, message = "Student classroom not found" });
        }



        [HttpPost]
        public IActionResult UpdateStatusAbsent(string studentId, string classcode)
        {
            // Convert studentId to int if it's an integer


            var studentClassroom = _context.Student_Clasrooms
                .FirstOrDefault(sc => sc.Classroom_Code.ToString() == classcode && sc.Student_ID == studentId);

            if (studentClassroom != null)
            {
                studentClassroom.Status = "Absent";


                TimeZoneInfo phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");

                // Convert the UTC time to Philippines Standard Time
                DateTime phTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, phTimeZone);

                // Update the Attendance_Time field with the Philippines time
                studentClassroom.Attendance_Time = phTime.ToString("yyyy-MM-dd hh:mm:ss tt");


                //studentClassroom.Attendance_Time = DateTime.Now.ToString();



                studentClassroom.Absent = (studentClassroom.Absent ?? 0) + 1;
                _context.SaveChanges();

                return Json(new { success = true, message = "Status updated successfully to absent" });
            }

            return Json(new { success = false, message = "Student classroom not found" });
        }
























        //[HttpPost]
        //public IActionResult UpdateStatus(string studentId, string classcode)
        //{
        //    // Convert studentId to int if it's an integer


        //    var studentClassroom = _context.Student_Clasrooms
        //        .FirstOrDefault(sc => sc.Classroom_Code.ToString() == classcode && sc.Student_ID == studentId);

        //    if (studentClassroom != null)
        //    {
        //        studentClassroom.Status = "Pending";
        //        _context.SaveChanges();

        //        return Json(new { success = true, message = "Status updated successfully" });
        //    }

        //    return Json(new { success = false, message = "Student classroom not found" });
        //}












        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ClassID,Attendance_Option,ClassCode,Educator_Name,ClassName,Section,Attendance_Start,Attendance_End,Days,EducatorEmail")] ClassroomDB classroomDB)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user != null)
        //    {
        //        var UserName = user.UserName;
        //        var educator = _context.Educators.FirstOrDefault(e => e.EmailEducator == UserName);
        //        if (educator != null)
        //        {
        //            classroomDB.Educator_Name = educator.FirstName + educator.LastName;
        //            classroomDB.EducatorEmail = UserName;
        //            Random random = new Random();

        //            // Generate a random 6-digit number
        //            int classCode = random.Next(100000, 999999);

        //            classroomDB.ClassCode = classCode;
        //            classroomDB.Attendance_Option = "Group Recognition";

        //            _context.Add(classroomDB);
        //            await _context.SaveChangesAsync();

        //            return RedirectToAction("Index", "Educator");
        //        }
        //    }

        //    return View(classroomDB);
        //}





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ClassID,Attendance_Option,ClassCode,Educator_Name,ClassName,Section,Attendance_Start,Attendance_End,Days,EducatorEmail")] ClassroomDB classroomDB)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(classroomDB);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index", "Educator"); // Redirect to the Index action of the Admin controller
        //    }
        //    return View(classroomDB);
        //}







        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ClassID,Attendance_Option,ClassCode,Educator_Name,ClassName,Section,Attendance_Start,Attendance_End,Days")] ClassroomDB classroomDB)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(classroomDB);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(classroomDB);
        //}




        // GET: ClassroomDBs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClassroomDBS == null)
            {
                return NotFound();
            }

            var classroomDB = await _context.ClassroomDBS.FindAsync(id);
            if (classroomDB == null)
            {
                return NotFound();
            }
            return View(classroomDB);
        }



        // POST: ClassroomDBs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassID,Attendance_Option,ClassCode,Educator_Name,ClassName,Section,Attendance_Start,Attendance_End,Days,EducatorEmail")] ClassroomDB classroomDB)
        {
            if (id != classroomDB.ClassID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classroomDB);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassroomDBExists(classroomDB.ClassID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Educator"); // Redirect to the Index action of the Admin controller
            }
            return View(classroomDB);
        }













        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ClassID,Attendance_Option,ClassCode,Educator_Name,ClassName,Section,Attendance_Start,Attendance_End,Days")] ClassroomDB classroomDB)
        //{
        //    if (id != classroomDB.ClassID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(classroomDB);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ClassroomDBExists(classroomDB.ClassID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(classroomDB);
        //}





        // GET: ClassroomDBs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClassroomDBS == null)
            {
                return NotFound();
            }

            var classroomDB = await _context.ClassroomDBS
                .FirstOrDefaultAsync(m => m.ClassID == id);
            if (classroomDB == null)
            {
                return NotFound();
            }

            return View(classroomDB);
        }







        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClassroomDBS == null)
            {
                return Problem("Entity set 'AuthDBContext.ClassroomDBS' is null.");
            }

            var classroomDB = await _context.ClassroomDBS.FindAsync(id);
            if (classroomDB != null)
            {
                _context.ClassroomDBS.Remove(classroomDB);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Educator"); // Redirect to the Index action of the Admin controller
        }











        //// POST: ClassroomDBs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.ClassroomDBS == null)
        //    {
        //        return Problem("Entity set 'ADMIN_STUDENTContext.ClassroomDBS'  is null.");
        //    }
        //    var classroomDB = await _context.ClassroomDBS.FindAsync(id);
        //    if (classroomDB != null)
        //    {
        //        _context.ClassroomDBS.Remove(classroomDB);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}







        private bool ClassroomDBExists(int id)
        {
            return (_context.ClassroomDBS?.Any(e => e.ClassID == id)).GetValueOrDefault();
        }
    }
}

