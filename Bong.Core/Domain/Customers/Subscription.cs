using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bong.Core.Domain.Customers
{
    public class Subscription : BaseEntity
    {
        public Guid SubscriptionGuid { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedOnUtc { get; set; }
    }
}
