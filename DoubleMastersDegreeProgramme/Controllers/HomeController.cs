using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace DoubleMastersDegreeProgramme.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Page"] = "HomeIndex";
            ViewData["Message"] = "Just Index";
            return View();
        }

        public IActionResult IndexReg()
        {
            ViewData["Page"] = "HomeIndex";
            ViewData["Message"] = "Registration in process";
            return View("Index");
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            // This method takes as parameters the code of the culture to 
            // be installed and the address to return to after installing the culture.

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return RedirectToAction("Index");
            //return LocalRedirect(returnUrl);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }
    }
}