using System.Data.Entity.ModelConfiguration;
using Bong.Core.Domain.Media;

namespace Bong.Data.Mapping.Media
{
    public class PictureMap : EntityTypeConfiguration<Picture>
    {
        public PictureMap()
        {
            this.ToTable("Picture");
            this.HasKey(p => p.Id);
            this.Property(p => p.PictureBinary).IsMaxLength();
            this.Property(p => p.Mime).IsRequired().HasMaxLength(40);
        }
    }
}
