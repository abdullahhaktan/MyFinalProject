using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ExperienceController : Controller
    {

        private readonly UserManager<WriterUser> _userManager;
        public ExperienceController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        ExperienceManager experienceManager = new ExperienceManager(new EfExperienceDal());
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);


            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var values = experienceManager.TGetList().Where(e=>e.User==user.UserName).ToList();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> AddExperience()
        {
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddExperience(Experience experience)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Eğer yeni resim yüklendiyse
            if (experience.Image != null && experience.Image.Length > 0)
            {
                var extension = Path.GetExtension(experience.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await experience.Image.CopyToAsync(stream);
                }

                experience.ImageUrl = "/userimage/" + newImageName;
            }

            

            experience.User = user.UserName;

            experienceManager.TAdd(experience);

            TempData["SuccessMessage"]= "Deneyim basariyla eklendi!";

            return RedirectToAction("Index");
        }
        public IActionResult DeleteExperience(int id)
        {
            var values = experienceManager.TGetByID(id);
            experienceManager.TDelete(values);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditExperience(int id)
        {
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var values = experienceManager.TGetByID(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> EditExperience(Experience experience)
        {
            
            // Eğer yeni resim yüklendiyse
            if (experience.Image != null && experience.Image.Length > 0)
            {
                var extension = Path.GetExtension(experience.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await experience.Image.CopyToAsync(stream);
                }

                experience.ImageUrl = "/userimage/" + newImageName;
            }
            else
            {
                var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
                string imageUrl = user1.ImageUrl;
                experience.ImageUrl = imageUrl;
            }
            TempData["SuccessMessage"] = "Deneyim basariyla guncellendi!";

            experienceManager.TUpdate(experience);
            return RedirectToAction("Index");
        }
    }
}