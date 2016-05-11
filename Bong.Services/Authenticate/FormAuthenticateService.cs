using System;
using System.Web;
using System.Web.Security;

using Bong.Core.Domain.Customers;
using Bong.Services.Customers;


namespace Bong.Services.Authenticate
{
    public class FormAuthenticateService : IAuthenticateService
    {
        #region Fields
                
        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;

        private Customer _loadedCustomer;

        #endregion

        #region ctor

        public FormAuthenticateService(HttpContextBase httpContext,
            ICustomerService customerService)
        {
            this._httpContext = httpContext;
            this._customerService = customerService;
        }

        #endregion

        #region Implementation the IAuthenticateService

        public void SignIn(Customer customer, bool persistentCookie)
        {

        }

        public void SignOut()
        {

        }

        public Customer GetAuthenticatedCustomer()
        {
            if (_loadedCustomer != null)
                return _loadedCustomer;

            if (_httpContext == null || _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated || !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var identity = (FormsIdentity)_httpContext.User.Identity;
            if (identity == null || identity.Ticket == null)
                throw new ArgumentException("identity ticket");

            var email = identity.Ticket.UserData;
            var customer = _customerService.GetCustomerByEmail(email);

            if (customer != null && customer.Active && !customer.Deleted && customer.UserType != UserType.Guest)
                _loadedCustomer = customer;
            return _loadedCustomer;
        }

        #endregion

        #region Utilites


        #endregion
    }
}
