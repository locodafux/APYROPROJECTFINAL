using APYROPROJECTFINAL.Areas.Identity.Data;
using APYROPROJECTFINAL.Data;
using APYROPROJECTFINAL.Migrations;
using APYROPROJECTFINAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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



            return _context.ClassroomDBS != null ?
                       View(await _context.ClassroomDBS.ToListAsync()) :
                       Problem("Entity set 'AuthDBContext.ClassroomDBS'  is null.");
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
                classroomDB.Attendance_Option = "Group Recognition";

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









        private bool ClassroomDBExists(int id)
        {
            return (_context.ClassroomDBS?.Any(e => e.ClassID == id)).GetValueOrDefault();
        }



    }
}
