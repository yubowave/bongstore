using System.Data.Entity.ModelConfiguration;
using Bong.Core.Domain.Goods;

namespace Bong.Data.Mapping.Goods
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            this.ToTable("Product");
            this.HasKey(p => p.Id);
            this.Property(p => p.Name).IsRequired().HasMaxLength(200);
            this.Property(p => p.Price).HasPrecision(18, 4).IsRequired();
            this.Property(p => p.OldPrice).HasPrecision(18, 4);
            this.Property(p => p.Weight).HasPrecision(18, 4);
            this.Property(p => p.Length).HasPrecision(18, 4);
            this.Property(p => p.Width).HasPrecision(18, 4);
            this.Property(p => p.Height).HasPrecision(18, 4);
        }
    }
}
