using System;
using System.Collections.Generic;
using System.Linq;

namespace Bong.Web.Models
{
    public class SimpleProductList : BaseViewModel
    {
        public SimpleProductList()
        {
            ProductViews = new List<ProductViewModel>();
        }

        public string Name { get; set; }

        public IList<ProductViewModel> ProductViews { get; set; }
    }
}