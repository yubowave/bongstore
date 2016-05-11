using System;
using System.Collections.Generic;
using Bong.Core.Domain.Goods;
using Bong.Core.Domain.Customers;

namespace Bong.Core.Domain.Orders
{
    public enum ShoppingCartType
    {
        ShoppingCart = 1,
        Wishlist = 2,
    }

    public class ShoppingCartItem : BaseEntity
    {
        public ShoppingCartType ShoppingCartType { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public decimal ItemPrice { get; set; }
        
        public int Quantity { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }
    }
}
