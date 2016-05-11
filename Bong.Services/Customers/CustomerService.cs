using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

using Bong.Core;
using Bong.Core.Collections;
using Bong.Core.Data;
using Bong.Core.Domain.Customers;
using Bong.Core.Domain.Goods;
using Bong.Core.Domain.Orders;

namespace Bong.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        #region Fields

        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<ProductReview> _productReviewRepository;

        #endregion

        #region Ctor

        public CustomerService(IRepository<Customer> customerRepository, IRepository<ProductReview> productReviewRepository)
        {
            this._customerRepository = customerRepository;
            this._productReviewRepository = productReviewRepository;
        }

        #endregion

        #region Customers Methods

        public virtual IPagedList<Customer> GetAllCustomers(DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null, UserType[] userTypes = null, string email = null,
            string username = null, bool loadOnlyWithShoppingCart = false, ShoppingCartType? sct = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _customerRepository.Table;
            if (createdFromUtc.HasValue)
                query = query.Where(c => createdFromUtc.Value <= c.CreatedOnUtc);
            if (createdToUtc.HasValue)
                query = query.Where(c => createdToUtc.Value >= c.CreatedOnUtc);
            query = query.Where(c => !c.Deleted);
            if (userTypes != null && userTypes.Length > 0)
                query = query.Where(c => userTypes.Contains(c.UserType));
            if (!String.IsNullOrWhiteSpace(email))
                query = query.Where(c => c.Email.Contains(email));
            if (!String.IsNullOrWhiteSpace(username))
                query = query.Where(c => c.Username.Contains(username));

            if (loadOnlyWithShoppingCart)
            {
                query = sct.HasValue ?
                    query.Where(c => c.ShoppingCartItems.Any(x => x.ShoppingCartType == sct)) :
                    query.Where(c => c.ShoppingCartItems.Any());
            }

            query = query.OrderByDescending(c => c.CreatedOnUtc);
            return new PagedList<Customer>(query, pageIndex, pageSize);
        }

        public virtual Customer GetCustomerById(int customerId)
        {
            if (customerId == 0)
                return null;

            return _customerRepository.GetById(customerId);
        }

        public virtual IList<Customer> GetCustomersByIds(int[] customerIds)
        {
            if (customerIds == null || customerIds.Length == 0)
                return new List<Customer>();

            var query = from c in _customerRepository.Table
                        where customerIds.Contains(c.Id)
                        select c;
            var customers = query.ToList();
            //sort by passed identifiers
            var sortedCustomers = new List<Customer>();
            foreach (int id in customerIds)
            {
                var customer = customers.Find(x => x.Id == id);
                if (customer != null)
                    sortedCustomers.Add(customer);
            }
            return sortedCustomers;
        }

        public virtual Customer GetCustomerByGuid(Guid customerGuid)
        {
            if (customerGuid == Guid.Empty)
                return null;

            var query = from c in _customerRepository.Table
                        where c.CustomerGuid == customerGuid
                        orderby c.Id
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        public virtual Customer GetCustomerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.Email == email
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        public virtual Customer GetCustomerByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.Username == username
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        public virtual Customer InsertGuestCustomer()
        {
            var customer = new Customer()
            {
                CustomerGuid = Guid.NewGuid(),
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                UserType = UserType.Guest,
            };
            _customerRepository.Insert(customer);

            return customer;
        }

        public virtual void InsertCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            _customerRepository.Insert(customer);
        }

        public virtual void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            _customerRepository.Update(customer);
        }

        public virtual void DeleteCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (customer.UserType == UserType.Admin)
                throw new BongException(string.Format("Admin account ({0}) could not be deleted", customer.Username));

            customer.Deleted = true;
            UpdateCustomer(customer);
        }

        public virtual int DeleteGuestCustomers(DateTime? createdFromUtc, DateTime? createdToUtc, bool onlyNoShoppingCart)
        {
            var query = _customerRepository.Table;
            if (createdFromUtc.HasValue)
                query = query.Where(c => createdFromUtc.Value <= c.CreatedOnUtc);
            if (createdToUtc.HasValue)
                query = query.Where(c => createdToUtc.Value >= c.CreatedOnUtc);
            query = query.Where(c => c.UserType == UserType.Guest);
            if (onlyNoShoppingCart)
                query = query.Where(c => !c.ShoppingCartItems.Any());
            //no product reviews
            query = from c in query
                    join pr in _productReviewRepository.Table on c.Id equals pr.CustomerId into c_pr
                    from pr in c_pr.DefaultIfEmpty()
                    where !c_pr.Any()
                    select c;
            //don't delete system accounts
            query = query.Where(c => !c.IsSystemAccount);

            //only distinct customers (group by ID)
            query = from c in query
                    group c by c.Id
                        into cGroup
                        orderby cGroup.Key
                        select cGroup.FirstOrDefault();
            query = query.OrderBy(c => c.Id);
            var customers = query.ToList();

            int numberOfDeletedCustomers = 0;
            foreach (var c in customers)
            {
                //delete from database
                _customerRepository.Delete(c);
                numberOfDeletedCustomers++;
            }
            return numberOfDeletedCustomers;
        }

        #endregion
    }
}
