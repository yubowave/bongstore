using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Bong.Core.Domain.Goods;
using Bong.Web.Areas.Admin.Models;

namespace Bong.Web.Areas.Admin
{
    public static class ModelExtension
    {
        public static CategoryModel ToCategoryModel(this Category category)
        {
            return new CategoryModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                PictureId = category.PictureId,
                ParentCategoryId = category.ParentCategoryId,
                Deleted = category.Deleted,
                ShowOnHomePage = category.ShowOnHomePage,
                CreatedOn = category.CreatedOnUtc,
                UpdatedOn = category.UpdatedOnUtc
            };
        }

        public static Category ToCategoryEntity(this CategoryModel model)
        {
            return new Category();
        }
    }
}