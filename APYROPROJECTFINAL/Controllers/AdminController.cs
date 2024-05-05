﻿using APYROPROJECTFINAL.Areas.Identity.Data;
using APYROPROJECTFINAL.Data;
using APYROPROJECTFINAL.Migrations;
using APYROPROJECTFINAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace APYROPROJECTFINAL.Controllers
{
    [Authorize(Roles = "Developer")]
    public class AdminController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger, IWebHostEnvironment hostingEnvironment, AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            this._userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {


            var attendanceOptions = await _context.ClassroomDBS
              .Select(c => c.Attendance_Option)
                 .Distinct()
             .ToListAsync();


            // Retrieve data for the dropdown list
            //var categories = GetCategoriesFromDatabase(); // Replace this with your own logic to get categories

            //// Create a SelectList to pass to the view
            //SelectList categoryList = new SelectList(categories, "Id", "Name");

            var categoryList = new SelectList(attendanceOptions);


            // Pass the SelectList to the view
            ViewBag.Categories = categoryList;



            return _context.ClassroomDBS != null ?
                       View(await _context.ClassroomDBS.ToListAsync()) :
                       Problem("Entity set 'AuthDBContext.ClassroomDBS'  is null.");
        }


      



        private List<Schoolyear> GetCategoriesFromDatabase()
        {
            return new List<Schoolyear>
        {
            new Schoolyear { Id = 1, Name = "2023 - 2024" },
            new Schoolyear { Id = 2, Name = "2022 - 2023" },
            new Schoolyear { Id = 3, Name = "2021 - 2022" },
            //new Schoolyear { Id = 4, Name = "4th Year" }
        };

        }
        




        public async Task <IActionResult> Backup()
        {
            var schoolyear = await _context.Student_Clasrooms
             .Select(c => c.Attendance_Option)
                .Distinct()
            .ToListAsync();

            var categoryList = new SelectList(schoolyear);


            // Pass the SelectList to the view
            ViewBag.Categories1 = categoryList;

            var classname = await _context.ClassroomDBS
             .Select(c => c.ClassName)
              .Distinct()
                     .ToListAsync();

            var categoryList1 = new SelectList(classname);

            ViewBag.Categories2 = categoryList1;

            // Assuming you have a dictionary mapping Classroom_Code to class names
            var classCodes = _context.ClassroomDBS.ToDictionary(c => c.ClassCode, c => new { ClassName = c.ClassName, EducatorName = c.Educator_Name , Section = c.Section });
            ViewBag.ClassCodes = classCodes;





            return _context.Student_Clasrooms != null ?
           View(await _context.Student_Clasrooms.ToListAsync()) :
            Problem("Entity set 'AuthDBContext.Student_Clasrooms'  is null.");

        }









        public IActionResult Create()
        {
          
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassID,Attendance_Option,ClassCode,Educator_Name,ClassName,Section,Attendance_Start,Attendance_End,Days,EducatorEmail")] Models.ClassroomDB classroomDB)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
          
                Random random = new Random();

                // Generate a random 6-digit number
                int classCode = random.Next(100000, 999999);

                classroomDB.ClassCode = classCode;
                //classroomDB.Attendance_Option = "Group Recognition";

                _context.Add(classroomDB);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Admin"); // Redirect to the Index action of the Admin controller
            }
            return View(classroomDB);
        }



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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassID,Attendance_Option,ClassCode,Educator_Name,ClassName,Section,Attendance_Start,Attendance_End,Days,EducatorEmail")] Models.ClassroomDB classroomDB)
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
                return RedirectToAction("Index", "Admin"); // Redirect to the Index action of the Admin controller
            }
            return View(classroomDB);
        }






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

            return RedirectToAction("Index", "Admin"); // Redirect to the Index action of the Admin controller
        }







        public async Task<IActionResult> EditStudentDataAsync(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == Id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStudentData(string Id, Student student)
        {
            if (Id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    // Retrieve the existing educator from the database
                    var existingStudent = await _context.Students.FirstOrDefaultAsync(e => e.Id == Id);

                    if (existingStudent != null)
                    {

                        // Update the properties
                        existingStudent.FirstName = student.FirstName;
                        existingStudent.LastName = student.LastName;
                        existingStudent.contactnumber = student.contactnumber;
                        existingStudent.EmailStudent = student.EmailStudent;
                        existingStudent.University = student.University;
                        existingStudent.IDnumber = student.IDnumber;
                        existingStudent.Username = student.Username;
                        existingStudent.PasswordStudent = student.PasswordStudent;
                        existingStudent.Section =student.Section;



                        // Update the entity in the database
                        _context.Update(existingStudent);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentIdExist(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Students", "Admin"); // Redirect to the Educators action of the Admin controller
            }
            return View(student);
        }











        public async Task<IActionResult> EditEducatorData(string? Id)
        {

            if (Id == null)
            {
                return NotFound();
            }


            var educator = await _context.Educators.FirstOrDefaultAsync(m => m.Id == Id);
            if (educator == null)
            {
                return NotFound();
            }

            return View(educator);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEducatorData(string Id, Educator educator)
        {
            if (Id != educator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    // Retrieve the existing educator from the database
                    var existingEducator = await _context.Educators.FirstOrDefaultAsync(e => e.Id == Id);

                    if (existingEducator != null)
                    {

                        // Update the properties
                        existingEducator.FirstName = educator.FirstName;
                        existingEducator.LastName = educator.LastName;
                        existingEducator.contactnumber = educator.contactnumber;
                        existingEducator.EmailEducator = educator.EmailEducator;
                        existingEducator.University = educator.University;
                        existingEducator.IDnumber = educator.IDnumber;
                        existingEducator.Username = educator.Username;
                        existingEducator.PasswordEducator = educator.PasswordEducator;

                        // Update the entity in the database
                        _context.Update(existingEducator);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EducatorIdExist(educator.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Educators", "Admin"); // Redirect to the Educators action of the Admin controller
            }
            return View(educator);
        }





        


        public async Task <IActionResult> AdminReports()
        {

            var EducatorsName = await _context.ClassroomDBS
              .Select(c => c.Educator_Name)
                 .Distinct()
             .ToListAsync();


            // Retrieve data for the dropdown list
            //var categories = GetCategoriesFromDatabase(); // Replace this with your own logic to get categories

            //// Create a SelectList to pass to the view
            //SelectList categoryList = new SelectList(categories, "Id", "Name");

            var categoryList = new SelectList(EducatorsName);


            // Pass the SelectList to the view
            ViewBag.Educators = categoryList;



            return _context.ClassroomDBS != null ?
                       View(await _context.ClassroomDBS.ToListAsync()) :
                       Problem("Entity set 'AuthDBContext.ClassroomDBS'  is null.");

        }


        public async Task<IActionResult> AdminViewReportAsync(int? ClassCode)
        {



            if (ClassCode == null)
            {
                return RedirectToAction("AdminErrorPage", "Admin");
            }

            var ViewReports = await _context.AttendanceReportDatanew.Where(m => m.EducatorClassCode == ClassCode).ToListAsync();

            if (ViewReports == null || !ViewReports.Any())
            {
                return RedirectToAction("AdminErrorPage", "Admin");
            }

            return View(ViewReports);
        }






        public async Task<IActionResult> AdminViewRecordsAsync(int? EducatorClassCode)
        {


            if (EducatorClassCode == null)
            {
                return RedirectToAction("AdminErrorPage", "Admin");
            }

            var datetbl = await _context.AttendanceReportDatanew
         .Select(m => m.DateTBL)
         .ToListAsync();

            var ViewRecords = await _context.AttendanceReportData
                .Where(m => m.Codeclass == EducatorClassCode && datetbl.Contains(m.AttendanceDateTBL))
                .ToListAsync();




            if (ViewRecords == null || !ViewRecords.Any())
            {
                return RedirectToAction("AdminErrorPage", "Admin");
            }



            return View(ViewRecords);
        }




        public async Task<IActionResult> AdminViewStudentsAsync(int? ClassCode)
        {



            if (ClassCode == null)
            {
                return RedirectToAction("AdminErrorPage", "Admin");
            }

            var ViewRecords = await _context.AttendanceReportData.Where(m => m.Codeclass == ClassCode).ToListAsync();

            if (ViewRecords == null || !ViewRecords.Any())
            {
                return RedirectToAction("AdminErrorPage", "Admin");
            }
            var ViewStudents = await _context.AttendanceReportDatanew.Where(m => m.EducatorClassCode == ClassCode).ToListAsync();


            ViewBag.ViewRecordsCount = ViewStudents.Count;




            return View(ViewRecords);
        }










        public IActionResult AdminErrorPage()
        {
            return View();
        }







        public async Task<IActionResult> SendEmailEducator(string? EmailEducator)
        {
            if (EmailEducator == null || _context.Educators == null)
            {
              
                return NotFound();
            }

            var educatoremail = await _context.Educators.FirstOrDefaultAsync(m => m.EmailEducator == EmailEducator);

            if (educatoremail == null)
            {
                
                return NotFound();
            }

            

            return View(educatoremail);
        }

     
  

















        public async Task<IActionResult> StudentEmailAsync(string? EmailStudent)
        {

            if (EmailStudent == null || _context.Students == null)
            {

                return NotFound();
            }

            var StudentEmail = await _context.Students.FirstOrDefaultAsync(m => m.EmailStudent == EmailStudent);

            if (StudentEmail == null)
            {

                return NotFound();
            }
            return View(StudentEmail);
        }







        public async Task<IActionResult> Educators()
        {

            return _context.Educators != null ?
            View(await _context.Educators.ToListAsync()) :
             Problem("Entity set 'AuthDBContext.Educators'  is null.");
        }


        public async Task<IActionResult> StudentsAsync()
        {
            return _context.Students != null ?
                         View(await _context.Students.ToListAsync()) :
                         Problem("Entity set 'AuthDBContext.Students'  is null.");
        }

        public async Task<IActionResult> Alluserslogs()
        {
            return _context.Userlogs != null ?
                         View(await _context.Userlogs.ToListAsync()) :
                         Problem("Entity set 'AuthDBContext.Userlogs'  is null.");
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




            var studentClassroomsDB = await _context.Student_Clasrooms
                .Where(s => s.Classroom_Code == classroomDB.ClassCode)
                .ToListAsync();

            var viewModel = new ClassroomViewModel
            {
                ClassroomDB = classroomDB,
                Students = studentClassroomsDB
            };



            return View(viewModel);
        }

        private bool EducatorIdExist(string Id)
        {
            return (_context.Educators?.Any(e => e.Id == Id)).GetValueOrDefault();
        }

        private bool EmailStudentExist(string EmailStudent)
        {
            return (_context.Students?.Any(e => e.EmailStudent == EmailStudent)).GetValueOrDefault();
        }


        private bool EmailEducatorExist(string EmailEducator)
        {
            return (_context.Educators?.Any(e => e.EmailEducator == EmailEducator)).GetValueOrDefault();
        }


        private bool ClassroomDBExists(int id)
        {
            return (_context.ClassroomDBS?.Any(e => e.ClassID == id)).GetValueOrDefault();
        }


        private bool StudentIdExist(string Id)
        {
            return (_context.Students?.Any(e => e.Id == Id)).GetValueOrDefault();
        }


    }
}
