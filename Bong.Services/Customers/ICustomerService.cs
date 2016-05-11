using System;
using System.Collections.Generic;

using Bong.Core.Collections;
using Bong.Core.Domain.Customers;
using Bong.Core.Domain.Orders;

namespace Bong.Services.Customers
{
    public interface ICustomerService
    {
        IPagedList<Customer> GetAllCustomers(DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null, UserType[] userTypes = null, string email = null, 
            string username = null, bool loadOnlyWithShoppingCart = false, ShoppingCartType? sct = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        Customer GetCustomerById(int customerId);

        IList<Customer> GetCustomersByIds(int[] customerIds);

        Customer GetCustomerByEmail(string email);

        Customer GetCustomerByGuid(Guid customerGuid);

        Customer GetCustomerByUsername(string username);

        Customer InsertGuestCustomer();

        void InsertCustomer(Customer customer);

        void UpdateCustomer(Customer customer);

        void DeleteCustomer(Customer customer);

        int DeleteGuestCustomers(DateTime? createdFromUtc, DateTime? createdToUtc, bool onlyWithoutShoppingCart);
    }
}
