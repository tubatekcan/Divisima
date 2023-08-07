using BL.Repositories;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Divisima.Areas.admin.Controllers
{
    [Authorize, Area("admin"),Route("/admin/[controller]/[action]/{id?}")]
    public class SlideController : Controller
    {
        IRepository<Slide> repoSlide;
        public SlideController(IRepository<Slide> _repoSlide)
        {
            repoSlide = _repoSlide;
        }

        public IActionResult Index()
        {
            return View(repoSlide.GetAll().OrderByDescending(x=>x.ID));
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(Slide model)
        {
            if (Request.Form.Files.Any())
            {
                if(!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","slide"))) Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "slide"));
                string dosyaAdi = (repoSlide.GetAll().Count()+1)+Request.Form.Files["Picture"].FileName;
                using (FileStream stream=new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "slide", dosyaAdi),FileMode.Create))
                {
                    await Request.Form.Files["Picture"].CopyToAsync(stream);
                }
                model.Picture = "/slide/" + dosyaAdi;
            }
            repoSlide.Add(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(repoSlide.GetBy(x=>x.ID==id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Slide model)
        {
            if (Request.Form.Files.Any())
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "slide"))) Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "slide"));
                string dosyaAdi = (repoSlide.GetAll().Count() + 1) + Request.Form.Files["Picture"].FileName;
                using (FileStream stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "slide", dosyaAdi), FileMode.Create))
                {
                    await Request.Form.Files["Picture"].CopyToAsync(stream);
                }
                model.Picture = "/slide/" + dosyaAdi;
            }
            repoSlide.Update(model);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Slide slide = repoSlide.GetBy(x => x.ID == id);
            repoSlide.Delete(slide);
            return RedirectToAction("Index");
        }
    }
}
