using System.Data.Entity.ModelConfiguration;
using Bong.Core.Domain.Customers;

namespace Bong.Data.Mapping.Customers
{
    public class SubscriptionMap : EntityTypeConfiguration<Subscription>
    {
        public SubscriptionMap()
        {
            this.ToTable("Subscription");
            this.HasKey(c => c.Id);

            this.Property(s => s.Email).IsRequired().HasMaxLength(255);
        }
    }
}
