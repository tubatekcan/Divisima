using BL.Repositories;
using DAL.Entities;
using Divisima.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Divisima.Areas.admin.Controllers
{
    [Area("admin"), Authorize]
    public class HomeController : Controller
    {
        IRepository<Admin> repoAdmin;
        public HomeController(IRepository<Admin> _repoAdmin)
        {
            repoAdmin = _repoAdmin;
        }

        [Route("/admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/admin/giris"), AllowAnonymous]
        public IActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [Route("/admin/giris"), AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Username, string Password, string ReturnUrl)
        {
            Admin admin = repoAdmin.GetBy(x => x.Username == Username && x.Password == GeneralTool.getMD5(Password)) ?? null;
            if (admin != null)//böyle bir admin varsa
            {
                List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.PrimarySid, admin.ID.ToString()),
                    new Claim(ClaimTypes.Name,admin.Name+" "+admin.Surname) 
                    //new Claim("Telefon",admin.Phone) 
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Divisima");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties { IsPersistent = true });
                admin.LastLoginDate = DateTime.Now;
                admin.LastLoginIPNo = HttpContext.Connection.RemoteIpAddress.ToString();
                repoAdmin.Update(admin, x => x.LastLoginDate, y => y.LastLoginIPNo);
                if (string.IsNullOrEmpty(ReturnUrl)) return Redirect("/admin");
                else return Redirect(ReturnUrl);
            }
            else TempData["Bilgi"] = "Kullanıcı adı veya şifre hatalı";
            return View();
        }

        [Route("/admin/cikis")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        //Linq: Language Integrated Query
        //int[] sayilar = { 5, 89, 65, 74, 254, 12 };
        ////int sayi = sayilar.FirstOrDefault();
        //int sayi = sayilar.FirstOrDefault(y=>y>75);
        //var sayilarim = sayilar.Where(x=>x!=65);

        //string[] isimler = { "Ali", "Ahmet", "Ayşe", "Kemal", "Cemal" };

        //var isimlerim = isimler.Where(x => !x.StartsWith("K"));
        //var isimlerim3 = isimler.Where(x => !x.EndsWith("K"));
        //var isimlerim2 = isimler.Where(x => !x.Contains("K"));

        //double[] Rakamlar = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        //var KucukRakamlar = from x in Rakamlar 
        //                    where x < 5
        //                    select x;

        //var _KucukRakamlar = Rakamlar.Where(x* => x < 5 && x>2);
        //void Hesapla(int x,int y) //Hesapla(4,5,6)
        //{
        //    int toplam = x + y;
        //}

        //void Hesapla(params int[] sayilar)//Hesapla(4,5,6) //Hesapla(0,1,4,5,6),//Hesapla(4,5,6,85,95)
        //{
        //    int toplam = 0;
        //    foreach (int s in sayilar) toplam += s;
        //}
    }


}
