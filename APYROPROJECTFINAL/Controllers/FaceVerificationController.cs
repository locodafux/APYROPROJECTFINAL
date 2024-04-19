using Microsoft.AspNetCore.Mvc;
using APYROPROJECTFINAL.Models;
using APYROPROJECTFINAL.Data;
using APYROPROJECTFINAL.Areas.Identity.Data;
using APYROPROJECTFINAL.Contants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using APYROPROJECTFINAL.Migrations;

namespace APYROPROJECTFINAL.Controllers
{
    [Authorize(Roles = "User")]
    public class FaceVerificationController : Controller
    {



        private readonly ILogger<FaceVerificationController> _logger;
        private readonly IWebHostEnvironment _hostEnviroment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _context;

        public FaceVerificationController(AuthDbContext context, IWebHostEnvironment hostEnvironment, ILogger<FaceVerificationController> logger , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostEnviroment = hostEnvironment;
            _logger = logger;
            this._userManager = userManager;
        }

        public async Task<IActionResult> UploadImageAsync([FromBody] UploadData uploadData)
        {
            var imagesFolder = Path.Combine(_hostEnviroment.WebRootPath, "images/Verified");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Get the number of existing image files in the folder
           // int nextImageNumber = Directory.GetFiles(imagesFolder, "*.png").Length + 1;



            //var fileName = $"{nextImageNumber}.png"; // Use the generated number as the filename
            var fileName = $"image_{DateTime.Now.Ticks}.jpg";
            var filePath = Path.Combine(imagesFolder, fileName);

            var data = Convert.FromBase64String(uploadData.ImageData.Split(',')[1]);

            // Save the image data to the specified file path
            System.IO.File.WriteAllBytes(filePath, data);






            //  Save image information to the database

            // Retrieve the current user
            var user = await _userManager.GetUserAsync(this.User);

            if (user != null && user is Student student)
            {
                // Update custom properties
                student.FileName = fileName;
                student.FilePath = filePath;



                var filePaths = await _context.Student_Clasrooms
                .Where(s => s.StudentEmail == user.Email)
                .ToListAsync();


                var studentClassroomsToUpdate = await _context.Student_Clasrooms
                .Where(s => s.StudentEmail == user.Email)
                 .ToListAsync();

                foreach (var classroom in studentClassroomsToUpdate)
                {
                    classroom.Filepath = filePath;
                    classroom.Filename = fileName;
                }

                await _context.SaveChangesAsync(); // Save changes to the database



                // Update the user in the database
                var result = await _userManager.UpdateAsync(student);

                if (result.Succeeded)
                {
                    // The properties have been updated successfully.
                    return Json(new { success = true });
                }
                else
                {
                    // Handle errors if the update fails (e.g., invalid data).
                    return Json(new { success = false, error = "Failed to update user." });
                }
            }
            else
            {
                // Handle the case where the user is not found or is not a Student.
                return Json(new { success = false, error = "User not found or not a Student." });
            }





            //  Save image information to the database
            //var image = new Student
            //{
            //    FileName = fileName,
            //    FilePath = filePath,
            //};
            //_context.Students.Add(image);
            //_context.SaveChanges();

            //return Json(new { success = true, imagePath = filePath });




        }




        public IActionResult Index()
        {
            return View();
        }
    }
}
