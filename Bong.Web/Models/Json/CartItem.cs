using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bong.Web.Models.Json
{
    public partial class CartItem
    {
        public CartItem()
        {
        }

        public int id { get; set; }

        public string imageurl { get; set; }

        public string productname { get; set; }

        public string producturl { get; set; }

        public decimal price { get; set; }

        public int quantity { get; set; }

        public decimal subtotal { get; set; }
    }
}