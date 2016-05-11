using System;
using System.Collections.Generic;

namespace Bong.Core.Domain.Goods
{
    public class Product : BaseEntity
    {
        public Product()
        {
            ProductCategories = new List<ProductCategory>();
            ProductReviews = new List<ProductReview>();
            ProductPictures = new List<ProductPicture>();
        }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal OldPrice { get; set; }

        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }

        public bool Deleted { get; set; }

        public bool ShowOnHomePage { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        public int RatingSum { get; set; }

        public int TotalReviews { get; set; }

        public virtual ICollection<ProductReview> ProductReviews { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }

        public virtual ICollection<ProductPicture> ProductPictures { get; set; }
    }
}
