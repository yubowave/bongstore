using System;
using System.Collections.Generic;
using Bong.Core.Domain.Media;

namespace Bong.Core.Domain.Goods
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int PictureId { get; set; }

        public bool Deleted { get; set; }

        public bool ShowOnHomePage { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// ParentCategoryId, root categery have a value of null for the property
        /// </summary>
        public int? ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }
    }
}
