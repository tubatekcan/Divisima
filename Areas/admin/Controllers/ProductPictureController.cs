using BL.Repositories;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Divisima.Areas.admin.Controllers
{
    [Authorize, Area("admin"),Route("/admin/[controller]/[action]/{id?}")]
    public class ProductPictureController : Controller
    {
        IRepository<ProductPicture> repoProductPicture;
        public ProductPictureController(IRepository<ProductPicture> _repoProductPicture)
        {
            repoProductPicture = _repoProductPicture;
        }

        public IActionResult Index(int id)
        {
            ViewBag.ProductID = id;
            return View(repoProductPicture.GetAll(x=>x.ProductID==id).OrderByDescending(x=>x.ID));
        }

        public IActionResult New(int productID)
        {
            ViewBag.ProductID = productID;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(ProductPicture model)
        {
            if (Request.Form.Files.Any())
            {
                if(!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","productPicture"))) Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "productPicture"));
                string dosyaAdi = (repoProductPicture.GetAll().Count()+1)+Request.Form.Files["Picture"].FileName;
                using (FileStream stream=new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "productPicture", dosyaAdi),FileMode.Create))
                {
                    await Request.Form.Files["Picture"].CopyToAsync(stream);
                }
                model.Picture = "/productPicture/" + dosyaAdi;
            }
            repoProductPicture.Add(model);
            return RedirectToAction("Index",new {id= model.ProductID});
        }

        public IActionResult Edit(int id)
        {
            return View(repoProductPicture.GetBy(x=>x.ID==id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductPicture model)
        {
            if (Request.Form.Files.Any())
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "productPicture"))) Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "productPicture"));
                string dosyaAdi = (repoProductPicture.GetAll().Count() + 1) + Request.Form.Files["Picture"].FileName;
                using (FileStream stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "productPicture", dosyaAdi), FileMode.Create))
                {
                    await Request.Form.Files["Picture"].CopyToAsync(stream);
                }
                model.Picture = "/productPicture/" + dosyaAdi;
            }
            repoProductPicture.Update(model);
            return RedirectToAction("Index", new { id = model.ProductID });
        }

        public IActionResult Delete(int id)
        {
            ProductPicture productPicture = repoProductPicture.GetBy(x => x.ID == id);
            repoProductPicture.Delete(productPicture);
            return RedirectToAction("Index", new { id = productPicture.ProductID });
        }
    }
}
