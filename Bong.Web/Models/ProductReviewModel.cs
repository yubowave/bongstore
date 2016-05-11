using System;
using System.Collections.Generic;

namespace Bong.Web.Models
{
    public class ProductReviewModel : BaseViewModel
    {
        public ProductReviewModel()
        {
            Customer = new CustomerModel();
        }

        public string Title { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public CustomerModel Customer { get; set; }
    }
}