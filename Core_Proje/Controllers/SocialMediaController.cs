using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{
    public class SocialMediaController : Controller
    {
        SocialMediaManager socialMediaManager = new SocialMediaManager(new EfSocialMediaDal());

        public string addIkon(SocialMedia p)
        {
            var ikonlar = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "instagram", "fa fa-instagram" },
                { "facebook", "fa fa-facebook" },
                { "twitter", "fa fa-twitter" },
                { "linkedin", "fa fa-linkedin" },
                { "youtube", "fa fa-youtube" },
                { "github", "fa fa-github" },
                { "tiktok", "fa fa-tiktok" },
                { "pinterest", "fa fa-pinterest" },
                { "snapchat", "fa fa-snapchat" },
                { "telegram", "fa fa-telegram" },
                { "reddit", "fa fa-reddit" },
                { "tumblr", "fa fa-tumblr" },
                { "medium", "fa fa-medium" },
            };
            if (!string.IsNullOrEmpty(p.Name) && ikonlar.ContainsKey(p.Name.Trim()))
            {
                p.Icon = ikonlar[p.Name.Trim()];
            }
            return p.Icon;
        }

        private readonly UserManager<WriterUser> _userManager;
        public SocialMediaController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var values = socialMediaManager.TGetList().Where(s=>s.User==user.UserName).ToList();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> AddSocialMedia()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSocialMedia(SocialMedia p)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            p.Icon = addIkon(p);

            p.Status = true;

            p.User = user.UserName;

            socialMediaManager.TAdd(p);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteSocialMedia(int id)
        {
            var values = socialMediaManager.TGetByID(id);
            socialMediaManager.TDelete(values);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditSocialMedia(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var values = socialMediaManager.TGetByID(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> EditSocialMedia(SocialMedia p)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            p.User = user.UserName;
            p.Icon = addIkon(p);
            socialMediaManager.TUpdate(p);
            return RedirectToAction("Index");
        }
    }
}
