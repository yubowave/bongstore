using System;
using System.Collections.Generic;

using Bong.Core.Domain.Orders;

namespace Bong.Core.Domain.Customers
{
    public enum UserType
    {
        Guest = 1,
        CustomerOnly = 2,
        Admin = 4,
    }

    public partial class Customer : BaseEntity
    {
        public Customer()
        {
            ShoppingCartItems = new List<ShoppingCartItem>();
        }

        public Guid CustomerGuid { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
        
        public int PasswordHashCode { get; set; }

        public UserType UserType { get; set; }

        public bool HasShoppingCartItems { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public bool IsSystemAccount 
        {
            get 
            {
                return this.UserType == UserType.Admin;
            }
        }

        public bool IsRegistered
        {
            get
            {
                if (Id > 0 && UserType != UserType.Guest)
                {
                    return true;
                }
                return false;
            }
        }

        public DateTime CreatedOnUtc { get; set; }

        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
