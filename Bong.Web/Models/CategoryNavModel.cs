using System.Collections.Generic;

namespace Bong.Web.Models
{
    public class CategoryNavModel : BaseViewModel
    {
        public CategoryNavModel()
        {
            Categories = new List<CategoryView>();
        }

        public int? CurrentCategoryId { get; set; }
        public List<CategoryView> Categories { get; set; }
    }
}