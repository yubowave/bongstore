using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using Bong.Web.Models;

namespace Bong.Web.Areas.Admin.Models
{
    public class CategoryModel : BaseViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int PictureId { get; set; }

        public int? ParentCategoryId { get; set; }

        public bool Deleted { get; set; }

        public bool ShowOnHomePage { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}