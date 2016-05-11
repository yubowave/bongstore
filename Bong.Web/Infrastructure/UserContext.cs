using System;
using System.Web;

using Bong.Core.Infrastructure;
using Bong.Core.Domain.Customers;
using Bong.Services.Customers;
using Bong.Services.Authenticate;

namespace Bong.Web.Infrastructure
{
    public class UserContext : IUserContext
    {
        #region Const

        private const string UserCookieName = "Bong.user";

        #endregion

        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;
        private readonly IAuthenticateService _authenticateService;

        private Customer _currentCustomer;

        #endregion

        #region ctor

        public UserContext(HttpContextBase httpContext, ICustomerService customerService,
            IAuthenticateService authenticateService)
        {
            _httpContext = httpContext;
            _customerService = customerService;
            _authenticateService = authenticateService;
        }

        #endregion

        #region Implementation

        public Customer CurrentUser
        {
            get
            {
                // if caller is not from httprequest, should make a specified customer 
                // for these clients 
                if (_httpContext == null)
                    return null;
                
                if (_currentCustomer != null)
                    return _currentCustomer;

                // for registered user
                Customer customer = null;
                if (customer == null || customer.Deleted || !customer.Active)
                {
                    customer = _authenticateService.GetAuthenticatedCustomer();
                }

                // for guest user
                if (customer == null || customer.Deleted || !customer.Active)
                {
                    var customerCookie = GetUserCookie();
                    if (customerCookie != null && !String.IsNullOrEmpty(customerCookie.Value))
                    {
                        Guid customerGuid;
                        if (Guid.TryParse(customerCookie.Value, out customerGuid))
                        {
                            var customerByCookie = _customerService.GetCustomerByGuid(customerGuid);
                            if (customerByCookie != null && !customerByCookie.IsRegistered)
                                customer = customerByCookie;
                        }
                    }
                }
                // create guest account
                if (customer == null || customer.Deleted || !customer.Active)
                {
                    customer = _customerService.InsertGuestCustomer();
                }

                //validation
                if (!customer.Deleted && customer.Active)
                {
                    SetUserCookie(customer.CustomerGuid);
                    _currentCustomer = customer;
                }

                return _currentCustomer;
            }
            set
            {
                SetUserCookie(value.CustomerGuid);
                _currentCustomer = value;
            }
        }

        #endregion

        #region Utilities

        protected virtual HttpCookie GetUserCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[UserCookieName];
        }

        protected virtual void SetUserCookie(Guid customerGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(UserCookieName);
                cookie.HttpOnly = true;
                cookie.Value = customerGuid.ToString();
                if (customerGuid == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddYears(1);
                }

                _httpContext.Response.Cookies.Remove(UserCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        #endregion
    }
}
