using BL.Repositories;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Divisima.Areas.admin.Controllers
{
    [Authorize, Area("admin"),Route("/admin/[controller]/[action]/{id?}")]
    public class CategoryController : Controller
    {
        IRepository<Category> repoCategory;
        public CategoryController(IRepository<Category> _repoCategory)
        {
            repoCategory = _repoCategory;
        }

        public IActionResult Index()
        {
            return View(repoCategory.GetAll().Include(x=>x.ParentCategory).OrderByDescending(x=>x.ID));
        }

        public IActionResult New()
        {
            ViewBag.Categories = repoCategory.GetAll().OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString() });
            return View();
        }

        [HttpPost]
        public IActionResult New(Category model)
        {
            repoCategory.Add(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = repoCategory.GetAll().OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString() });
            return View(repoCategory.GetBy(x=>x.ID==id));
        }

        [HttpPost]
        public IActionResult Edit(Category model)
        {
            repoCategory.Update(model);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Category category = repoCategory.GetBy(x => x.ID == id);
            repoCategory.Delete(category);
            return RedirectToAction("Index");
        }
    }
}
