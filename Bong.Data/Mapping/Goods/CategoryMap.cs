using System.Data.Entity.ModelConfiguration;
using Bong.Core.Domain.Goods;


namespace Bong.Data.Mapping.Goods
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            this.ToTable("Category");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(200);

            this.HasOptional(cc => cc.ParentCategory)
                .WithMany(pc => pc.SubCategories)
                .HasForeignKey(cc => cc.ParentCategoryId)
                .WillCascadeOnDelete(false);
         }
    }
}
