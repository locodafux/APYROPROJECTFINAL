using Microsoft.AspNetCore.Mvc;

namespace APYROPROJECTFINAL.Controllers
{
    public class MainPageController : Controller
    {
  
        public IActionResult MainHomePage()
        {
            return View();
        }

        public IActionResult Documentation()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();

        }

        public IActionResult Policies() 
        { 

            return View();
        }

        public IActionResult ContactUs()
        {

            return View();
        }



        public IActionResult EducatorStudentSignUp()
        {
            return View();
        }
    }
}
