using System;
using System.Collections.Generic;
using System.Linq;
using Bong.Core;
using Bong.Core.Data;
using Bong.Core.Domain.Goods;
using Bong.Core.Domain.Customers;
using Bong.Core.Domain.Orders;

using Bong.Services.Goods;
using Bong.Services.Customers;

namespace Bong.Services.Orders
{
    public partial class ShoppingCartService : IShoppingCartService
    {
        #region Fields

        private readonly IRepository<ShoppingCartItem> _sciRepository;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public ShoppingCartService(IRepository<ShoppingCartItem> sciRepository,
            IProductService productService, ICustomerService customerService)
        {
            this._sciRepository = sciRepository;
            this._productService = productService;
            this._customerService = customerService;
        }

        #endregion

        #region Methods

        public virtual ShoppingCartItem FindShoppingCartItemInTheCart(IList<ShoppingCartItem> shoppingCart,
            ShoppingCartType shoppingCartType, Product product)
        {
            if (shoppingCart == null)
                throw new ArgumentNullException("shoppingCart");

            if (product == null)
                throw new ArgumentNullException("product");

            foreach (var sci in shoppingCart.Where(a => a.ShoppingCartType == shoppingCartType))
            {
                if (sci.ProductId == product.Id)
                   return sci;
            }
            return null;
        }

        public virtual void AddToCart(Customer customer, Product product, ShoppingCartType shoppingCartType, int quantity)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (product == null)
                throw new ArgumentNullException("product");

            var cart = customer.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == shoppingCartType)
                .ToList();

            var shoppingCartItem = FindShoppingCartItemInTheCart(cart, shoppingCartType, product);

            if (shoppingCartItem != null)
            {
                //update existing shopping cart item
                shoppingCartItem.Quantity = shoppingCartItem.Quantity + quantity;
                shoppingCartItem.UpdatedOnUtc = DateTime.UtcNow;
                _customerService.UpdateCustomer(customer);
            }
            else
            {
                //new shopping cart item
                DateTime now = DateTime.UtcNow;
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartType = shoppingCartType,
                    Product = product,
                    ItemPrice = product.Price,
                    Quantity = quantity,
                    CreatedOnUtc = now,
                    UpdatedOnUtc = now
                };
                customer.ShoppingCartItems.Add(shoppingCartItem);
                _customerService.UpdateCustomer(customer);

                //updated "HasShoppingCartItems" property used for performance optimization
                customer.HasShoppingCartItems = customer.ShoppingCartItems.Count > 0;
                _customerService.UpdateCustomer(customer);
            }
        }

        public virtual void UpdateShoppingCartItem(Customer customer, int shoppingCartItemId, int quantity)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var shoppingCartItem = customer.ShoppingCartItems.FirstOrDefault(sci => sci.Id == shoppingCartItemId);
            if (shoppingCartItem != null)
            {
                if (quantity > 0)
                {
                    shoppingCartItem.Quantity = quantity;
                    shoppingCartItem.UpdatedOnUtc = DateTime.UtcNow;
                    _customerService.UpdateCustomer(customer);
                }
                else
                {
                    DeleteShoppingCartItem(shoppingCartItem);
                }
            }
        }

        public virtual void DeleteShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem == null)
                throw new ArgumentNullException("shoppingCartItem");

            var customer = shoppingCartItem.Customer;

            //delete item
            _sciRepository.Delete(shoppingCartItem);

            //reset "HasShoppingCartItems" property used for performance optimization
            customer.HasShoppingCartItems = customer.ShoppingCartItems.Count > 0;
            _customerService.UpdateCustomer(customer);
        }

        public virtual int DeleteExpiredShoppingCartItems(DateTime olderThanUtc)
        {
            var query = from sci in _sciRepository.Table
                        where sci.UpdatedOnUtc < olderThanUtc
                        select sci;

            var cartItems = query.ToList();
            foreach (var cartItem in cartItems)
                _sciRepository.Delete(cartItem);
            return cartItems.Count;
        }

        #endregion
    }
}
