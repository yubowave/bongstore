using System.Data.Entity.ModelConfiguration;
using Bong.Core.Domain.Goods;

namespace Bong.Data.Mapping.Goods
{
    public class ProductCategoryMap : EntityTypeConfiguration<ProductCategory>
    {
        public ProductCategoryMap()
        {
            this.ToTable("Product_Category_Mapping");
            this.HasKey(p => p.Id);

            this.HasRequired(pc => pc.Product)
                .WithMany(p=>p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            this.HasRequired(pc => pc.Category)
                .WithMany()
                .HasForeignKey(pp => pp.CategoryId);
        }
    }
}
