
namespace Bong.Core.Domain.Goods
{
    public partial class ProductCategory : BaseEntity
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public bool IsFeaturedProduct { get; set; }

        public int ShowOrder { get; set; }
    }
}
