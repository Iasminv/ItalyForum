using System.Diagnostics;
using ItalyForum.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItalyForum.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
