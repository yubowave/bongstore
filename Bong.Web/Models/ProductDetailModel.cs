using System;
using System.Collections.Generic;

namespace Bong.Web.Models
{
    public class ProductDetailModel : BaseViewModel
    {
        public ProductDetailModel()
        {
            DefaultPictureModel = new PictureModel();
            PictureModels = new List<PictureModel>();
            CategoryBreadcrumb = new List<CategoryView>();
        }

        // Basic
        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
        public string PriceDisplay
        {
            get
            {
                if (Price > 0)
                    return Price.ToString("C");

                return string.Empty;
            }
        }

        public decimal OldPrice { get; set; }
        public string OldPriceDisplay
        {
            get
            {
                if (OldPrice > 0)
                    return OldPrice.ToString("C");

                return string.Empty;
            }
        }

        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }

        public bool ShowOnHomePage { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        public int RatingSum { get; set; }

        public int TotalReviews { get; set; }

        public decimal AverageRate
        {
            get
            {
                if (TotalReviews == 0) return 0;

                return RatingSum / TotalReviews * 20;
            }
        }

        // picture
        public PictureModel DefaultPictureModel { get; set; }
        public IList<PictureModel> PictureModels { get; set; }

        public IList<CategoryView> CategoryBreadcrumb { get; set; }
    }
}