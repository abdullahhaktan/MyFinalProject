using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core_Proje.Areas.Writer.Controllers
{
    [Area("Writer")]
    [AllowAnonymous]

    public class DashboardController : Controller
    {
        private readonly UserManager<WriterUser> _userManager;

        AboutManager about = new AboutManager(new EfAboutDal());

        FeatureManager feature = new FeatureManager(new EfFeatureDal());


        public DashboardController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.v = values.Name + " " + values.Surname;

            string userName = values.UserName;

            //Weather APi
            string api = "57665488183f4444aafe4f35da221366";
            string connection = "http://api.openweathermap.org/data/2.5/weather?q=istanbul&mode=xml&lang=tr&units=metric&appid=" + api;
            XDocument document = XDocument.Load(connection);
            ViewBag.v5 = document.Descendants("temperature").ElementAt(0).Attribute("value").Value;

            //statistics
            Context c = new Context();
            //ViewBag.v1 = c.WriterMessages.Where(m => m.Receiver  == values.Email /*&& m.User == userName*/).Count();
            ViewBag.v2 = c.Announcements.Count();
            ViewBag.v3 = c.Users.Count();
            ViewBag.v4 = c.Skills.Where(s=>s.User == userName).Count();
            return View();
        }

        public async Task<IActionResult> Vitrin()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);

            var aboutValues = about.TGetList().Where(about => about.User == values.UserName).ToList();
            var featureValues = feature.TGetList().Where(feature => feature.User == values.UserName).ToList();

            if ((aboutValues.Count==0) || (featureValues.Count==0))
            {
                HttpContext.Session.SetString("SuccessMessage", "İşlem başarılı!");
                return RedirectToAction("Index", new { msg = "İşlem başarılı!" });
            }

            return RedirectToAction("Index", "Default");
        }
        
        public async Task<IActionResult> Admin()
        {

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
/*
 http://api.openweathermap.org/data/2.5/weather?q=istanbul&mode=xml&lang=tr&units=metric&appid=14ad2aba611dbef9c504b82a127794c5
 */