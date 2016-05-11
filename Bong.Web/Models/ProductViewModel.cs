
namespace Bong.Web.Models
{
    public class ProductViewModel : BaseViewModel
    {
        public ProductViewModel()
        {
            PictureModel = new PictureModel();
        }

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

        public PictureModel PictureModel { get; set; }
    }
}