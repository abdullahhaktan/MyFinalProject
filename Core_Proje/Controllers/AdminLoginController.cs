using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core_Proje.Controllers
{
    [AllowAnonymous]
    public class AdminLoginController : Controller
    {

        Context context = new Context();
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        //this controller is disabled

        public async Task<IActionResult> ToDashBoard(Admin a)
        {
            var user = context.Admins.Where(u => u.userName == a.userName && u.password == a.password).FirstOrDefault();
            if (user != null)
            {
                // Giriş yapan kullanıcı için claims oluştur
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.userName)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Kullanıcıyı authenticate et
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString("yetki", "1");

                return RedirectToAction("Index", "Dashboard");
            }

            else
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış.");
            }
            return RedirectToAction("Index");
        }
    }
}
