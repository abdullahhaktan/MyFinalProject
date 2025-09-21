using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly UserManager<WriterUser> _userManager;
        public PortfolioController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        public string userName()
        {
            var user = User.Identity.Name;

            return user;
        }

        PortfolioManager portfolioManager = new PortfolioManager(new EfPortfolioDal());
        


        public async Task<IActionResult> Index()
        {
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;


            ViewBag.imageUrl = user1.ImageUrl;

            var userName = user1.UserName;

            var values = portfolioManager.TGetList().Where(p=>p.User == userName).ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddPortfolio()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPortfolio(Portfolio portfolio)
        {

            // Eğer yeni resim yüklendiyse
            if (portfolio.Image != null && portfolio.Image.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.Image.CopyToAsync(stream);
                }

                portfolio.ImageUrl = "/userimage/" + newImageName;
            }

            // Eğer yeni resim yüklendiyse
            if (portfolio.Image1 != null && portfolio.Image1.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.Image1.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.Image1.CopyToAsync(stream);
                }

                portfolio.ImageUrl1 = "/userimage/" + newImageName;
            }

            // Eğer yeni resim yüklendiyse
            if (portfolio.screenShotImage != null && portfolio.screenShotImage.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.screenShotImage.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.screenShotImage.CopyToAsync(stream);
                }

                portfolio.screenShotImageUrl = "/userimage/" + newImageName;
            }

            if (portfolio.screenShotImage1 != null && portfolio.screenShotImage1.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.screenShotImage1.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.screenShotImage1.CopyToAsync(stream);
                }

                portfolio.screenShotImageUrl1 = "/userimage/" + newImageName;
            }

            if (portfolio.screenShotImage2 != null && portfolio.screenShotImage2.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.screenShotImage2.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.screenShotImage2.CopyToAsync(stream);
                }

                portfolio.screenShotImageUrl2 = "/userimage/" + newImageName;
            }

            portfolio.User = userName();
            PortfolioValidator validations = new PortfolioValidator();
            ValidationResult results = validations.Validate(portfolio);
            if (results.IsValid)
            {
                portfolioManager.TAdd(portfolio);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            return RedirectToAction("Index","Portfolio");
        }

        public IActionResult DeletePortfolio(int id)
        {
            var values = portfolioManager.TGetByID(id);
            portfolioManager.TDelete(values);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditPortfolio(int id)
        {
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var values = portfolioManager.TGetByID(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> EditPortfolio(Portfolio portfolio)
        {
            var userName1 = userName();

            var ImageUrl = portfolioManager.TGetList().Where(x => x.User == userName1 && x.PortfolioID == portfolio.PortfolioID).Select(x => x.ImageUrl).FirstOrDefault();

            var ImageUrl1 = portfolioManager.TGetList().Where(x => x.User == userName1 && x.PortfolioID == portfolio.PortfolioID).Select(x => x.ImageUrl1).FirstOrDefault();


            var screenShortImageUrl = portfolioManager.TGetList().Where(x => x.User == userName1 && x.PortfolioID == portfolio.PortfolioID).Select(x => x.screenShotImageUrl).FirstOrDefault();

            var screenShortImageUrl1 = portfolioManager.TGetList().Where(x => x.User == userName1 && x.PortfolioID == portfolio.PortfolioID).Select(x => x.screenShotImageUrl1).FirstOrDefault();

            var screenShortImageUrl2 = portfolioManager.TGetList().Where(x => x.User == userName1 && x.PortfolioID == portfolio.PortfolioID).Select(x => x.screenShotImageUrl2).FirstOrDefault();

            // Eğer yeni resim yüklendiyse
            if (portfolio.Image != null && portfolio.Image.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.Image.CopyToAsync(stream);
                }

                portfolio.ImageUrl = "/userimage/" + newImageName;
            }

            else
            {
                portfolio.ImageUrl = ImageUrl;
            }

            // Eğer yeni resim yüklendiyse
            if (portfolio.Image1 != null && portfolio.Image1.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.Image1.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.Image1.CopyToAsync(stream);
                }

                portfolio.ImageUrl1 = "/userimage/" + newImageName;
            }

            else
            {
                portfolio.ImageUrl1 = ImageUrl1;
            }


            // Eğer yeni resim yüklendiyse
            if (portfolio.screenShotImage != null && portfolio.screenShotImage.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.screenShotImage.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.screenShotImage.CopyToAsync(stream);
                }

                portfolio.screenShotImageUrl = "/userimage/" + newImageName;
            }

            else
            {
                portfolio.screenShotImageUrl = screenShortImageUrl;
            }

            if (portfolio.screenShotImage1 != null && portfolio.screenShotImage1.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.screenShotImage1.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.screenShotImage1.CopyToAsync(stream);
                }

                portfolio.screenShotImageUrl1 = "/userimage/" + newImageName;
            }

            if (portfolio.screenShotImage2 != null && portfolio.screenShotImage2.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.screenShotImage2.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.screenShotImage2.CopyToAsync(stream);
                }

                portfolio.screenShotImageUrl2 = "/userimage/" + newImageName;
            }


            portfolio.User = userName();

            PortfolioValidator validations = new PortfolioValidator();
            ValidationResult results = validations.Validate(portfolio);
            if (results.IsValid)
            {
                portfolioManager.TUpdate(portfolio);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return RedirectToAction("EditPortfolio", "Portfolio");
        }
    }
}
