using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bong.Web.Models
{
    public partial class HeaderModel : BaseViewModel
    {
        public int ShoppingCartItems { get; set; }

        public int WishListItems { get; set; }

        public bool IsRegistered { get; set; }
    }
}