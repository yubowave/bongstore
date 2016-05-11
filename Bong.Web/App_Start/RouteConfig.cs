using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bong.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // define routes here
            routes.MapRoute("Home",
                "",
                new { controller = "Home", action = "Index" },
                new[] { "Bong.Web.Controllers" });

            //login
            routes.MapRoute("Login",
                            "login/",
                            new { controller = "Customer", action = "Login" },
                            new[] { "Bong.Web.Controllers" });
            //register
            routes.MapRoute("Register",
                            "register/",
                            new { controller = "Customer", action = "Register" },
                            new[] { "Bong.Web.Controllers" });
            //logout
            routes.MapRoute("Logout",
                            "logout/",
                            new { controller = "Customer", action = "Logout" },
                            new[] { "Bong.Web.Controllers" });

            //shopping cart
            routes.MapRoute("ShoppingCart",
                            "cart/",
                            new { controller = "ShoppingCart", action = "Cart" },
                            new[] { "Bong.Web.Controllers" });
            //wishlist
            routes.MapRoute("Wishlist",
                            "wishlist/",
                            new { controller = "ShoppingCart", action = "Wishlist" },
                            new[] { "Bong.Web.Controllers" });

            /* AJAX action start */
            // get cart
            routes.MapRoute("GetCart",
                "GetCart/",
                new { controller = "ShoppingCart", action = "GetCart" },
                new[] { "Bong.Web.Controllers" });

            routes.MapRoute("UpdateCart",
                "UpdateCart/",
                new { controller = "ShoppingCart", action = "UpdateCart" },
                new[] { "Bong.Web.Controllers" });

            //Subscribe
            routes.MapRoute("Subscribe",
                            "subscribe/",
                            new { controller = "Common", action = "Subscribe" },
                            new[] { "Bong.Web.Controllers" });

            // Add to cart
            routes.MapRoute("AddProductToCart",
                "addproducttocart/{productId}/{shoppingCartTypeId}/{quantity}",
                new { controller = "ShoppingCart", action = "AddProductToCart" },
                new { productId = @"\d+", shoppingCartTypeId = @"\d+", quantity = @"\d+" },
                new[] { "Bong.Web.Controllers" });

            routes.MapRoute("AddProductToCart-Detail",
                "addproducttocart-detail/{productId}/{shoppingCartTypeId}",
                new { controller = "ShoppingCart", action = "AddProductToCart_Detail" },
                new { productId = @"\d+", shoppingCartTypeId = @"\d+" },
                new[] { "Bong.Web.Controllers" });

            /* AJAX action end */

            // Category Detail
            routes.MapRoute("Category",
                "category/{categoryid}",
                new { controller = "Category", action = "CategoryDetails" },
                new[] { "Bong.Web.Controllers" });

            // Product Detail
            routes.MapRoute("Product",
                         "Product/{productId}",
                         new { controller = "Product", action = "ProductDetails" },
                         new[] { "Bong.Web.Controllers" });

            // recently viewed products
            routes.MapRoute("RecentViewedProducts",
                "recentviewedproducts/",
                new { controller = "Product", action = "RecentViewedProducts" },
                new[] { "Bong.Web.Controllers" });

            // NewProducts
            routes.MapRoute("NewProducts",
                "newproducts/",
                new { controller = "Product", action = "LatestAddedProducts" },
                new[] { "Bong.Web.Controllers" });

            //Privacy Policy
            routes.MapRoute("PrivacyPolicy",
                            "privacypolicy",
                            new { controller = "Home", action = "PrivacyPolicy" },
                            new[] { "Bong.Web.Controllers" });

            //Shipping Returns
            routes.MapRoute("ShippingReturns",
                            "shippingreturns",
                            new { controller = "Home", action = "ShippingReturns" },
                            new[] { "Bong.Web.Controllers" });

            //contact us
            routes.MapRoute("ContactUs",
                            "contactus",
                            new { controller = "Home", action = "Contact" },
                            new[] { "Bong.Web.Controllers" });

            //about us
            routes.MapRoute("AboutUs",
                            "abouttus",
                            new { controller = "Home", action = "About" },
                            new[] { "Bong.Web.Controllers" });

            //sitemap
            routes.MapRoute("Sitemap",
                            "sitemap",
                            new { controller = "Common", action = "Sitemap" },
                            new[] { "Bong.Web.Controllers" });

            //product search
            routes.MapRoute("ProductSearch",
                            "search/",
                            new { controller = "Product", action = "Search" },
                            new[] { "Bong.Web.Controllers" });

            // not page found
            routes.MapRoute("NoPageFound",
                            "not-page-found",
                            new { controller = "Common", action = "NoPageFound" },
                            new[] { "Bong.Web.Controllers" });

            // general url pattern
            routes.MapRoute(
                name: "Default",                                // route name
                url: "{controller}/{action}/{id}",              // url pattern
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Bong.Web.Controllers" }    // namespace of controller
            );
        }
    }
}
