using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{
    public class AboutController : Controller
    {
        AboutManager aboutManager = new AboutManager(new EfAboutDal());

        private readonly UserManager<WriterUser> _userManager;
        public AboutController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;
            var values = aboutManager.TGetList().Where(a=>a.User==user.UserName).FirstOrDefault();

            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> Index(About about)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Eğer yeni resim yüklendiyse
            if (about.Image != null && about.Image.Length > 0)
            {
                var extension = Path.GetExtension(about.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await about.Image.CopyToAsync(stream);
                }

                about.ImageUrl = "/userimage/" + newImageName;
            }

            about.User = user.UserName;
            aboutManager.TUpdate(about);
            TempData["SuccessMessage"] = "Hakkında kısmı basariyla kaydedildi , kontrol etmek için siteyi ziyaret edebilirsiniz.";

            return RedirectToAction("Index");
        }
    }
}
