using System;
using System.Collections.Generic;
using Bong.Core.Domain.Goods;
using Bong.Core.Domain.Customers;
using Bong.Core.Domain.Orders;

namespace Bong.Services.Orders
{
    public partial interface IShoppingCartService
    {
        void AddToCart(Customer customer, Product product, ShoppingCartType shoppingCartType, int quantity);

        void UpdateShoppingCartItem(Customer customer, int shoppingCartItemId, int quantity);

        void DeleteShoppingCartItem(ShoppingCartItem shoppingCartItem);
        
        int DeleteExpiredShoppingCartItems(DateTime olderThanUtc);
    }
}