using System.Data.Entity.ModelConfiguration;
using Bong.Core.Domain.Goods;

namespace Bong.Data.Mapping.Goods
{
    public class ProductPictureMap : EntityTypeConfiguration<ProductPicture>
    {
        public ProductPictureMap()
        {
            this.ToTable("Product_Picture_Mapping");
            this.HasKey(p => p.Id);

            this.HasRequired(pp => pp.Product)
                .WithMany(p=>p.ProductPictures)
                .HasForeignKey(pp => pp.ProductId);

            this.HasRequired(pp => pp.Picture)
                .WithMany()
                .HasForeignKey(pp => pp.PictureId);
        }
    }
}
