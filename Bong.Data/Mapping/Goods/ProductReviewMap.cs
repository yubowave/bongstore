using System.Data.Entity.ModelConfiguration;
using Bong.Core.Domain.Goods;

namespace Bong.Data.Mapping.Goods
{
    public class ProductReviewMap : EntityTypeConfiguration<ProductReview>
    {
        public ProductReviewMap()
        {
            this.ToTable("ProductReview");
            this.HasKey(v => v.Id);
            this.Property(v => v.Title).IsRequired().HasMaxLength(200);

            this.HasRequired(v => v.Customer)
                .WithMany()
                .HasForeignKey(v => v.CustomerId);

            this.HasRequired(v => v.Product)
                .WithMany(p => p.ProductReviews)
                .HasForeignKey(v => v.ProductId);
        }
    }
}
