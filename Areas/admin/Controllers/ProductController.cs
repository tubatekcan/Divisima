using BL.Repositories;
using DAL.Entities;
using Divisima.Areas.admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Divisima.Areas.admin.Controllers
{
    [Authorize, Area("admin"),Route("/admin/[controller]/[action]/{id?}")]
    public class ProductController : Controller
    {
        IRepository<Product> repoProduct;
        IRepository<Brand> repoBrand;
        IRepository<Category> repoCategory;
        IRepository<ProductCategory> repoProductCategory;
        public ProductController(IRepository<Product> _repoProduct, IRepository<Brand> _repoBrand, IRepository<Category> _repoCategory, IRepository<ProductCategory> _repoProductCategory)
        {
            repoProduct = _repoProduct;
            repoBrand = _repoBrand;
            repoCategory = _repoCategory;
            repoProductCategory = _repoProductCategory;
        }

        public IActionResult Index()
        {
            return View(repoProduct.GetAll().Include(x=>x.Brand).Include(x=>x.ProductCategories).ThenInclude(x=>x.Category).OrderByDescending(x=>x.ID));
        }

        public IActionResult New()
        {
            ProductVM productVM = new() {
                Brands = repoBrand.GetAll().OrderBy(x => x.Name),
                Product = new(),
                Categories = repoCategory.GetAll().OrderBy(x => x.Name)
            };
            return View(productVM);
        }

        [HttpPost]
        public IActionResult New(ProductVM model)
        {
            repoProduct.Add(model.Product);
            foreach (int categoryID in model.SelectedCategoryID)
            {
                repoProductCategory.Add(new ProductCategory {ProductID= model.Product.ID,CategoryID= categoryID });
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            ProductVM productVM = new()
            {
                Brands = repoBrand.GetAll().OrderBy(x => x.Name),
                Product = repoProduct.GetBy(x => x.ID == id),
                Categories = repoCategory.GetAll().OrderBy(x => x.Name),
                SelectedCategoryID=repoProductCategory.GetAll(x=>x.ProductID==id).Select(s=>s.CategoryID).ToArray()
            };
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Edit(ProductVM model)
        {
            repoProduct.Update(model.Product);

            //silme
            repoProductCategory.DeleteAll(repoProductCategory.GetAll(x=>x.ProductID==model.Product.ID));

            //ekleme
            foreach (int categoryID in model.SelectedCategoryID)
            {
                repoProductCategory.Add(new ProductCategory { ProductID = model.Product.ID, CategoryID = categoryID });
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Product product = repoProduct.GetBy(x => x.ID == id);
            repoProduct.Delete(product);
            return RedirectToAction("Index");
        }
    }
}
