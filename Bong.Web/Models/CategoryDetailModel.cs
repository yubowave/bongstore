using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bong.Web.Models
{
    public class CategoryDetailModel : BaseViewModel
    {
        public CategoryDetailModel()
        {
            PictureModel = new PictureModel();
            FeaturedProducts = new List<ProductViewModel>();
            Products = new List<ProductViewModel>();
            SubCategories = new List<CategoryView>();
            CategoryBreadcrumb = new List<CategoryView>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public PictureModel PictureModel { get; set; }

        public IList<CategoryView> CategoryBreadcrumb { get; set; }
        
        public IList<CategoryView> SubCategories { get; set; }

        public IList<ProductViewModel> FeaturedProducts { get; set; }

        public IList<ProductViewModel> Products { get; set; }
    }
}