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
    public class ServiceController : Controller
    {
        ServiceManager serviceManager = new ServiceManager(new EfServiceDal());

        private readonly UserManager<WriterUser> _userManager;
        public ServiceController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var values = serviceManager.TGetList().Where(s => s.User == user.UserName).ToList();
            return View(values);
        }
        [HttpGet]
        public async Task<IActionResult> AddService()
        {
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddService(Service service)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);


            // Eğer yeni resim yüklendiyse
            if (service.Image != null && service.Image.Length > 0)
            {
                var extension = Path.GetExtension(service.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await service.Image.CopyToAsync(stream);
                }

                service.ImageUrl = "/userimage/" + newImageName;
            }

            service.User = user.UserName;
            serviceManager.TAdd(service);
            TempData["SuccessMessage"] = "Hizmet basariyla eklendi";
            return RedirectToAction("Index");
        }
        public IActionResult DeleteService(int id)
        {
            var values = serviceManager.TGetByID(id);
            serviceManager.TDelete(values);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            var values = serviceManager.TGetByID(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> EditService(Service service)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Eğer yeni resim yüklendiyse
            if (service.Image != null && service.Image.Length > 0)
            {
                var extension = Path.GetExtension(service.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await service.Image.CopyToAsync(stream);
                }

                service.ImageUrl = "/userimage/" + newImageName;
            }
            else
            {
                service.ImageUrl = user.ImageUrl;
            }

            service.User = user.UserName; // Set the current user's name

            serviceManager.TUpdate(service);
            TempData["SuccessMessage"]= "Hizmet basariyla guncellendi!";
            return RedirectToAction("Index");
        }
    }
}
