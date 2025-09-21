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
    public class TestimonialController : Controller
    {
        TestimonialManager testimonialManager = new TestimonialManager(new EfTestimonialDal());


        private readonly UserManager<WriterUser> _userManager;
        public TestimonialController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            ViewBag.nameSurname = user.Name + " " + user.Surname;

            var values = testimonialManager.TGetList().Where(t=>t.User == user.UserName).ToList();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> AddTestimonial()
        {
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTestimonial(Testimonial testimonial)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (testimonial.Image != null && testimonial.Image.Length > 0)
            {
                var extension = Path.GetExtension(testimonial.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await testimonial.Image.CopyToAsync(stream);
                }

                testimonial.ImageUrl = "/userimage/" + newImageName;
            }

            testimonial.User = user.UserName;

            testimonialManager.TAdd(testimonial);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteTestimonial(int id)
        {
            var values = testimonialManager.TGetByID(id);
            testimonialManager.TDelete(values);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditTestimonial(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            ViewBag.nameSurname = user.Name + " " + user.Surname;
            var values = testimonialManager.TGetByID(id);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> EditTestimonial(Testimonial testimonial)
        {
            if (testimonial.Image != null && testimonial.Image.Length > 0)
            {
                var extension = Path.GetExtension(testimonial.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await testimonial.Image.CopyToAsync(stream);
                }

                testimonial.ImageUrl = "/userimage/" + newImageName;
            }

            else
            {
                var referencee = testimonialManager.TGetByID(testimonial.TestimonialID);
                testimonial.ImageUrl = referencee.ImageUrl; // Eğer resim yüklenmemişse mevcut resmi koru
            }

            testimonialManager.TUpdate(testimonial);
            return RedirectToAction("Index");
        }
    }
}
