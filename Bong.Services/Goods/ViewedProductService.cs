using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bong.Core.Domain.Goods;

namespace Bong.Services.Goods
{
    public class ViewedProductService : IViewedProductService
    {
        #region Fields

        private const int MaxViewdNumber = 10;
        private const string ViewedCookieName = "Bong.ViewedProducts";
        private const string ViewedProductKey = "ViewedProductIds";

        private readonly HttpContextBase _httpContext;
        private readonly IProductService _productService;

        #endregion

        #region Ctor

        public ViewedProductService(HttpContextBase httpContext, IProductService productService)
        {
            _httpContext = httpContext;
            this._productService = productService;
        }

        #endregion

        #region Utilities

        protected IList<int> GetRecentlyViewedProductsIds(int number = int.MaxValue)
        {
            var productIds = new List<int>();
            var recentlyViewedCookie = _httpContext.Request.Cookies.Get(ViewedCookieName);
            if ((recentlyViewedCookie == null) || (recentlyViewedCookie.Values == null))
                return productIds;

            string[] values = recentlyViewedCookie.Values.GetValues(ViewedProductKey);
            if (values == null)
                return productIds;
            foreach (string productId in values)
            {
                int prodId = int.Parse(productId);
                if (!productIds.Contains(prodId))
                {
                    productIds.Add(prodId);
                    if (productIds.Count >= number)
                        break;
                }

            }
            return productIds;
        }

        #endregion

        #region Implementation of IViewedProduct interfeace

        public virtual void AddViewdProduct(int productId)
        {
            var oldProductIds = GetRecentlyViewedProductsIds();
            var newProductIds = new List<int>();
            newProductIds.Add(productId);
            foreach (int oldProductId in oldProductIds)
            {
                if (oldProductId != productId)
                    newProductIds.Add(oldProductId);
            }

            // Cookie
            var recentlyViewedCookie = _httpContext.Request.Cookies.Get(ViewedCookieName);
            if (recentlyViewedCookie == null)
            {
                recentlyViewedCookie = new HttpCookie(ViewedCookieName);
                recentlyViewedCookie.HttpOnly = true;
            }
            recentlyViewedCookie.Values.Clear();

            int i = 1;
            foreach (int newProductId in newProductIds)
            {
                recentlyViewedCookie.Values.Add(ViewedProductKey, newProductId.ToString());
                if (i == MaxViewdNumber)
                    break;
                i++;
            }
            recentlyViewedCookie.Expires = DateTime.Now.AddDays(10.0);
            _httpContext.Response.Cookies.Set(recentlyViewedCookie);
        }

        public virtual void ClearAll()
        {
            _httpContext.Response.Cookies.Remove(ViewedCookieName);
        }

        public virtual IList<Product> GetRecentViewedProducts(int count = int.MaxValue)
        {
            var productIds = GetRecentlyViewedProductsIds(count);
            var products = _productService.GetProductsByIds(productIds.ToArray());

            return products;
        }
        
        #endregion
    }
}
