using System;
using System.Collections.Generic;

using Bong.Core.Domain.Customers;

namespace Bong.Core.Domain.Goods
{
    public class ProductReview : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
