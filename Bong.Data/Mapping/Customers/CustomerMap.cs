using System.Data.Entity.ModelConfiguration;
using Bong.Core.Domain.Customers;

namespace Bong.Data.Mapping.Customers
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            this.ToTable("Customer");
            this.HasKey(c => c.Id);
            this.Property(u => u.Username).HasMaxLength(168);
            this.Property(u => u.Email).HasMaxLength(488);
        }
    }
}
