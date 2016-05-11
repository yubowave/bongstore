using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bong.Web.Models
{
    public class CategoryView : BaseViewModel
    {
        public CategoryView()
        {
            SubCategories = new List<CategoryView>();
        }

        public string Name { get; set; }

        public PictureModel PictureModel { get; set; }

        public IList<CategoryView> SubCategories { get; set; }
    }
}