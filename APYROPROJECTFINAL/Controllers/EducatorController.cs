using APYROPROJECTFINAL.Contants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using APYROPROJECTFINAL.Areas.Identity.Data;
using APYROPROJECTFINAL.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using APYROPROJECTFINAL.Models;

namespace APYROPROJECTFINAL.Controllers
{

    [Authorize(Roles = "Educator")]
    public class EducatorController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _context;


        public EducatorController( UserManager<ApplicationUser> userManager , ILogger<HomeController> logger, AuthDbContext context)
        {
            _logger = logger;
            this._userManager = userManager;
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User); // Retrieve the currently logged-in user

            if (user != null)
            {
                // Find the educator with the matching Educator_Name
                var educator = _context.ClassroomDBS.FirstOrDefault(e => e.EducatorEmail == user.UserName);

                if (educator != null)
                {
                    // If educator exists, return the view with the matched ClassroomDBS records
                    var classrooms = _context.ClassroomDBS.Where(c => c.EducatorEmail == user.UserName).ToList();
                    return View(classrooms);
                }
                else
                {
                    // Educator account exists, but no classrooms created
                    return View(new List<ClassroomDB>()); // Return an empty list
                }
            }

            // If no user is logged in, return an empty view
            return View(new List<ClassroomDB>());
        }










        //public async Task<IActionResult> Index()
        //{
        //    var user = await _userManager.GetUserAsync(User); // Retrieve the currently logged-in user

        //    if (user != null)
        //    {
        //        // Find the educator with the matching Educator_Name
        //        var educator = _context.ClassroomDBS.FirstOrDefault(e => e.EducatorEmail == user.UserName);

        //        if (educator != null)
        //        {
        //            // If educator exists, return the view with the matched ClassroomDBS records
        //            var classrooms = _context.ClassroomDBS.Where(c => c.EducatorEmail == user.UserName).ToList();
        //            return View(classrooms);
        //        }
        //        else
        //        {
        //            // Educator account exists, but no classrooms created
        //            return View(new List<ClassroomDB>()); // Return an empty list
        //        }
        //    }

        //    // If no user is logged in, return an empty view
        //    return View(new List<ClassroomDB>());
        //}










        //public async Task<IActionResult> Index()
        //{

        //    return _context.ClassroomDBS != null ?
        //                View(await _context.ClassroomDBS.ToListAsync()) :
        //                Problem("Entity set 'ADMIN_STUDENTContext.ClassroomDBS'  is null.");
        //}









        // GET: ClassroomDBs/Details/5




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






        public async Task<IActionResult> Details(int? id)
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


        public IActionResult Logout()
        {
            return View();
        }







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
        public async Task<IActionResult> Create([Bind("ClassID,Attendance_Option,ClassCode,Educator_Name,ClassName,Section,Attendance_Start,Attendance_End,Days")] ClassroomDB classroomDB)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classroomDB);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classroomDB);
        }



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
        public async Task<IActionResult> Edit(int id, [Bind("ClassID,Attendance_Option,ClassCode,Educator_Name,ClassName,Section,Attendance_Start,Attendance_End,Days")] ClassroomDB classroomDB)
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
                return RedirectToAction(nameof(Index));
            }
            return View(classroomDB);
        }








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
                return Problem("Entity set 'ADMIN_STUDENTContext.ClassroomDBS' is null.");
            }

            var classroomDB = await _context.ClassroomDBS.FindAsync(id);
            if (classroomDB != null)
            {
                _context.ClassroomDBS.Remove(classroomDB);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Admin"); // Redirect to the Index action of the Admin controller
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
















        //public IActionResult Index()
        //{
        //    ViewData["UserID"] = _userManager.GetUserId(this.User);
        //    return View();
        //}
    }
}
