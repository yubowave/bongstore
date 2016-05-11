using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Bong.Core.Domain.Goods;
using Bong.Core.Domain.Orders;
using Bong.Services.Goods;
using Bong.Services.Orders;
using Bong.Web.Infrastructure;
using Bong.Core.Infrastructure;
using Bong.Web.Models;
using Bong.Web.Models.Json;
using Bong.Services.Media;
using Bong.Core.Configuration;
using Bong.Web.Models.Bind;

namespace Bong.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        #region Fields

        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IUserContext _userContext;
        private readonly IPictureService _pictureService;
        private readonly SystemSetting _sysSetting;

        #endregion

        #region ctor

        public ShoppingCartController(IProductService productService, IPictureService pictureService,
            IShoppingCartService shoppingCartService, SystemSetting sysSetting, IUserContext userContext)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _userContext = userContext;
            _pictureService = pictureService;
            _sysSetting = sysSetting;
        }

        #endregion

        #region Helper

        [NonAction]
        protected virtual void PrepareShoppingCartModel(ShoppingCartModel model, IList<ShoppingCartItem> items)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (items == null)
                throw new ArgumentNullException("cart");

            if (items.Count == 0) return;

            foreach (var item in items)
            {
                var itemModel = new ShoppingCartModel.ShoppingCartItemModel()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    ItemPrice = item.ItemPrice,
                    Quantity = item.Quantity,
                    SubTotal = item.ItemPrice * item.Quantity,
                };

                // picture
                var picture = _pictureService.GetPicturesByProductId(itemModel.ProductId, 1).FirstOrDefault();
                int imageSize = _sysSetting.ProductDetailPicSize;

                // PictureModel
                PictureModel pictureModel = new PictureModel
                {
                    ImageUrl = _pictureService.GetPictureUrl(picture, imageSize),
                    FullSizeImageUrl = _pictureService.GetPictureUrl(picture),
                    Title = string.Format("Show detail for {0}", itemModel.ProductName),
                    AlternateText = string.Format("Image of {0}", itemModel.ProductName)
                };
                itemModel.Picture = pictureModel;
                model.Items.Add(itemModel);
            }
        }

        #endregion

        #region Json Action

        [NonAction]
        public JsonResult GetShoppingCertItems(ShoppingCartType type = ShoppingCartType.ShoppingCart)
        {
            var cart = _userContext.CurrentUser.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == type)
                .ToList();

            var result = new ArrayList();
            foreach (var item in cart)
            {
                // picture
                var picture = _pictureService.GetPicturesByProductId(item.ProductId, 1).FirstOrDefault();
                int imageSize = _sysSetting.ProductDetailPicSize;

                var itemModel = new 
                {
                    id = item.Id,
                    imageurl = _pictureService.GetPictureUrl(picture, imageSize),
                    productname = item.Product.Name,
                    producturl = Url.RouteUrl("Product", new { productId = item.ProductId }),
                    price = item.ItemPrice,
                    quantity = item.Quantity,
                };
                result.Add(itemModel);
            }
            return Json(result.ToArray());
        }

        [HttpGet]
        public JsonResult GetCart()
        {
            JsonResult result = GetShoppingCertItems(ShoppingCartType.ShoppingCart);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return  result;
        }

        [HttpGet]
        public JsonResult GetWishlist()
        {
            return GetShoppingCertItems(ShoppingCartType.Wishlist);
        }

        [NonAction]
        public JsonResult AddToCart(int productId, int shoppingCartTypeId, int quantity)
        {
            var shoppingCartType = (ShoppingCartType)shoppingCartTypeId;

            // quantity
            if (quantity <= 0)
            {
                return Json(new
                {
                    success = false,
                    message = "Quantity should be positive number."
                });
            }

            // product 
            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Don't product found with the specified ID."
                });
            }

            // add to shoppingcart
            _shoppingCartService.AddToCart(_userContext.CurrentUser, product, shoppingCartType, quantity);

            string message = string.Empty;
            if (shoppingCartType == ShoppingCartType.ShoppingCart)
                message = "The product has been added to your shopping cart.";
            else
                message = "The product has been added to your wishlist.";

            switch (shoppingCartType)
            {
                case ShoppingCartType.Wishlist:
                    {
                        int wishItems = _userContext.CurrentUser.ShoppingCartItems.Where(x=>x.ShoppingCartType==ShoppingCartType.Wishlist).ToList().Count;
                        return Json(new
                        {
                            success = true,
                            message = message,
                            updatewishlistinfo = wishItems
                        });
                    }
                case ShoppingCartType.ShoppingCart:
                default:
                    {
                        int cartItems = _userContext.CurrentUser.ShoppingCartItems.Where(x => x.ShoppingCartType == ShoppingCartType.ShoppingCart).ToList().Count;
                        return Json(new
                        {
                            success = true,
                            message = message,
                            updatecartinfo = cartItems
                        });
                    }
            }
        }

        [HttpPost]
        public ActionResult AddProductToCart(int productId, int shoppingCartTypeId, int quantity = 1)
        {
            return AddToCart(productId, shoppingCartTypeId, quantity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddProductToCart_Detail(int productId, int shoppingCartTypeId, string strquantity)
        {
            // quantity
            int quantity;
            bool isNum = int.TryParse(strquantity, out quantity);
            if (!isNum || quantity <= 0)
            {
                return Json(new
                {
                    success = false,
                    message = "Quantity should be positive number."
                });
            }
            return AddToCart(productId, shoppingCartTypeId, quantity);
        }

        #endregion

        #region Partial Views

        public ActionResult ShoppingCartLinks()
        {
            return PartialView();
        }

        #endregion

        #region Views

        //public ActionResult Cart()
        //{
        //    var cart = _userContext.CurrentUser.ShoppingCartItems
        //        .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
        //        .ToList();

        //    var model = new ShoppingCartModel();
        //    PrepareShoppingCartModel(model, cart);
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //[ActionName("Cart")]
        //public ActionResult UpdateCart(FormCollection form)
        //{
        //    var cart = _userContext.CurrentUser.ShoppingCartItems
        //        .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
        //        .ToList();

        //    var allIdsdeleting = form["removefromcart"] != null ?
        //        form["removefromcart"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList()
        //        : new List<int>();

        //    foreach (var cartItem in cart)
        //    {
        //        if (allIdsdeleting.Contains(cartItem.Id))
        //        {
        //            _shoppingCartService.DeleteShoppingCartItem(cartItem);
        //        }
        //        else
        //        {
        //            foreach (string formKey in form.AllKeys)
        //                if (formKey.Equals(string.Format("qty{0}", cartItem.Id), StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    int newQuantity = cartItem.Quantity;
        //                    if (int.TryParse(form[formKey], out newQuantity))
        //                    {
        //                        _shoppingCartService.UpdateShoppingCartItem(_userContext.CurrentUser, cartItem.Id, newQuantity);
        //                    }
        //                    break;
        //                }
        //        }
        //    }

        //    cart = _userContext.CurrentUser.ShoppingCartItems
        //        .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
        //        .ToList();

        //    var model = new ShoppingCartModel();
        //    PrepareShoppingCartModel(model, cart);
        //    return View(model);
        //}

        public ActionResult Cart()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ActionName("Cart")]
        public ActionResult UpdateCart([FromJson]IEnumerable<CartItem> scitems)
        {
            if (scitems == null) return View();
 
            var cart = _userContext.CurrentUser.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                .ToList();

            foreach (var cartItem in cart)
            {
                var scitem = scitems.Where(sci => sci.id == cartItem.Id).SingleOrDefault();

                if (scitem == null)
                {
                    _shoppingCartService.DeleteShoppingCartItem(cartItem);
                }
                else if (cartItem.Quantity != scitem.quantity)
                {
                    _shoppingCartService.UpdateShoppingCartItem(_userContext.CurrentUser, scitem.id, scitem.quantity);
                }
            }
            return View();
        }

        public ActionResult Wishlist()
        {
            var cart = _userContext.CurrentUser.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.Wishlist)
                .ToList();

            var model = new ShoppingCartModel();
            PrepareShoppingCartModel(model, cart);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ActionName("Wishlist")]
        public ActionResult UpdateWishList(FormCollection form)
        {
            var cart = _userContext.CurrentUser.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.Wishlist)
                .ToList();

            var allIdsdeleting = form["removefromlist"] != null ?
                form["removefromlist"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList()
                : new List<int>();

            foreach (var cartItem in cart)
            {
                if (allIdsdeleting.Contains(cartItem.Id))
                {
                    _shoppingCartService.DeleteShoppingCartItem(cartItem);
                }
            }

            cart = _userContext.CurrentUser.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.Wishlist)
                .ToList();

            var model = new ShoppingCartModel();
            PrepareShoppingCartModel(model, cart);
            return View(model);
        }

        #endregion
    }
}