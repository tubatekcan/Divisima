using DAL.Entities;

namespace Divisima.Areas.admin.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public int[] SelectedCategoryID { get; set; }
    }
}
