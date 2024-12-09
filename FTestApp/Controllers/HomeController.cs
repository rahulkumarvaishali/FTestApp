using Facebook;
using FTestApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FTestApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public ActionResult Index()
        //{
        //    var fb = new FacebookClient();
        //    var loginUrl = fb.GetLoginUrl(new
        //    {
        //        client_id = "582574390819245",
        //        redirect_uri = "https://localhost:7252/Home/FacebookRedirect",
        //        scope = "public_profile,email"
        //    });
        //    ViewBag.Url = loginUrl;

        //    return View();
        //}

        public ActionResult Index(string Name, string Email)
        {
            // Pass the data to the view via ViewBag
            ViewBag.Name = Name;
            ViewBag.Email = Email;

            // Generate login URL for Facebook (already in your original code)
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "582574390819245",
                redirect_uri = "https://localhost:7252/Home/FacebookRedirect",
                scope = "public_profile,email"
            });
            ViewBag.Url = loginUrl;

            return View();
        }

        //public ActionResult FacebookRedirect(string code)
        //{
        //    var fb = new FacebookClient();
        //    dynamic result = fb.Get("/oauth/access_token", new
        //    {
        //        client_id = "582574390819245",
        //        client_secret = "fe227d8a5a1dd1f99d76d653ce0f7989",
        //        redirect_uri = "https://localhost:7252/Home/FacebookRedirect",
        //        code = code
        //    });

        //    fb.AccessToken = result.access_token;

        //    dynamic me = fb.Get("/me?fields=name,email");
        //    string name = me.name;
        //    string email = me.email;
        //    return RedirectToAction("Index");
        //}

        public ActionResult FacebookRedirect(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Get("/oauth/access_token", new
            {
                client_id = "582574390819245",
                client_secret = "fe227d8a5a1dd1f99d76d653ce0f7989",
                redirect_uri = "https://localhost:7252/Home/FacebookRedirect",
                code = code
            });

            fb.AccessToken = result.access_token;

            dynamic me = fb.Get("/me?fields=name,email");
            string name = me.name;
            string email = me.email;

            // Pass the data to the Welcome page
            ViewBag.Name = name;
            ViewBag.Email = email;

            // Redirect to the new Welcome page
            return View("Welcome");
        }


        public async Task<IActionResult> Logout()
        {
            try
            {
                // Clear session data
                HttpContext.Session.Clear(); // ASP.NET Core session clearing

                // Clear authentication cookies using Cookie Authentication in ASP.NET Core
                await HttpContext.SignOutAsync(); // Signs out the user and clears the authentication cookie

                // Optional: Redirect to a logout confirmation page or home page
                return RedirectToAction("Index", "Home"); // Redirect to the home page or login page
            }
            catch (Exception ex)
            {
                // Log the error in production (use a logging library like NLog or Serilog)
                _logger.LogError($"Error during logout: {ex.Message}");

                // Redirect to an error view
                ViewBag.ErrorMessage = $"Error during logout: {ex.Message}";
                return View("Error"); // Redirect to an error view
            }
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
