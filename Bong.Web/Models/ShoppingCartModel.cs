using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bong.Web.Models
{
    public partial class ShoppingCartModel : BaseViewModel
    {
        public ShoppingCartModel()
        {
            Items = new List<ShoppingCartItemModel>();
        }

        public IList<ShoppingCartItemModel> Items { get; set; }

        public decimal Total
        {
            get
            {
                decimal total = 0;

                foreach (var item in Items)
                {
                    total += item.SubTotal;
                }
                return total;
            }
        }

        #region Nested Classes

        public partial class ShoppingCartItemModel : BaseViewModel
        {
            public ShoppingCartItemModel()
            {
                Picture = new PictureModel();
            }

            public PictureModel Picture { get; set; }

            public int ProductId { get; set; }

            public string ProductName { get; set; }

            public decimal ItemPrice { get; set; }

            public decimal SubTotal { get; set; }

            public int Quantity { get; set; }
        }

        #endregion
    }
}