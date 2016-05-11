using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Bong.Core.Helper;
using Bong.Core.Domain.Customers;
using Bong.Services.Customers;
using Bong.Web.Models;
using Bong.Core.Infrastructure;
using Bong.Core.Domain.Orders;

namespace Bong.Web.Controllers
{
    public class CommonController : Controller
    {
        #region Fields
        private readonly ISubscribeService _subscribeService;
        private readonly IUserContext _userContext;

        #endregion

        #region ctor

        public CommonController(ISubscribeService subscribeService, IUserContext userContext)
        {
            _subscribeService = subscribeService;
            _userContext = userContext;
        }

        #endregion

        #region Views

        public ActionResult Sitemap()
        {
            return View();
        }
        
        public ActionResult NoPageFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;

            return View();
        }

        public ActionResult Error(Exception error)
        {
            return View(error);
        }

        #endregion
        
        #region Partial Views

        [ChildActionOnly]
        public ActionResult HeaderLinks()
        {
            HeaderModel model = new HeaderModel()
            {
                ShoppingCartItems = _userContext.CurrentUser.ShoppingCartItems
                    .Where(x => x.ShoppingCartType == ShoppingCartType.ShoppingCart).ToList().Count,
                WishListItems = _userContext.CurrentUser.ShoppingCartItems
                    .Where(x => x.ShoppingCartType == ShoppingCartType.Wishlist).ToList().Count,
                IsRegistered = _userContext.CurrentUser.IsRegistered,
            };

            return PartialView(model);
        }

        public ActionResult SubscribeWidget()
        {
            return PartialView(new SubscriptionModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Subscribe(string email)
        {
            string result;
            bool success = false;

            if (!CommonHelper.IsValidEmail(email))
                result = "Wrong Email";
            else
            {
                //subscribe/unsubscribe
                email = email.Trim();

                var subscription = _subscribeService.GetSubscriptionByEmail(email);
                if (subscription != null)
                {
                    result = "Thank you for signing up! We appreciate your interest.";
                }
                else
                {
                    subscription = new Subscription()
                    {
                        SubscriptionGuid = Guid.NewGuid(),
                        Email = email,
                        Active = true,
                        CreatedOnUtc = DateTime.UtcNow
                    };
                    _subscribeService.InsertSubscription(subscription);
                    result = "Thank you for signing up! We appreciate your interest.";
                }
                success = true;
            }

            return Json(new
            {
                Success = success,
                Result = result,
            });
        }

        #endregion
    }
}