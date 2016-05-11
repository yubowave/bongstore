using Bong.Core.Domain.Customers;

namespace Bong.Services.Authenticate
{
    public interface IAuthenticateService
    {
        void SignIn(Customer customer, bool persistentCookie);

        void SignOut();

        Customer GetAuthenticatedCustomer();
    }
}
