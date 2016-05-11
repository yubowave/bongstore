using System.Data.Entity.ModelConfiguration;
using Bong.Core.Domain.Orders;

namespace Bong.Data.Mapping.Orders
{
    public class ShoppingCartItemMap : EntityTypeConfiguration<ShoppingCartItem>
    {
        public ShoppingCartItemMap()
        {
            this.ToTable("ShoppingCartItem");
            this.HasKey(c => c.Id);
            this.Property(i => i.ItemPrice).HasPrecision(18, 4);

            this.HasRequired(sc => sc.Customer)
                .WithMany(c => c.ShoppingCartItems)
                .HasForeignKey(sc => sc.CustomerId);

            this.HasRequired(sc => sc.Product)
                .WithMany()
                .HasForeignKey(sc => sc.ProductId);
        }
    }
}