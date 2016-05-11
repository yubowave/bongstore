using System;
using System.Collections.Generic;

using Bong.Core.Domain.Media;

namespace Bong.Core.Domain.Goods
{
    public class ProductPicture : BaseEntity
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int PictureId { get; set; }
        public virtual Picture Picture { get; set; }

        public int ShowOrder { get; set; }
    }
}
