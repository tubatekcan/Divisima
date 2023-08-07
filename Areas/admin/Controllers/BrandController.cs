using BL.Repositories;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Divisima.Areas.admin.Controllers
{
    [Authorize, Area("admin"),Route("/admin/[controller]/[action]/{id?}")]
    public class BrandController : Controller
    {
        IRepository<Brand> repoBrand;
        public BrandController(IRepository<Brand> _repoBrand)
        {
            repoBrand = _repoBrand;
        }

        public IActionResult Index()
        {
            return View(repoBrand.GetAll().OrderByDescending(x=>x.ID));
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Brand model)
        {
            repoBrand.Add(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(repoBrand.GetBy(x=>x.ID==id));
        }

        [HttpPost]
        public IActionResult Edit(Brand model)
        {
            repoBrand.Update(model);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Brand brand = repoBrand.GetBy(x => x.ID == id);
            repoBrand.Delete(brand);
            return RedirectToAction("Index");
        }
    }
}
