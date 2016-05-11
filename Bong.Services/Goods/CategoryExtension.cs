using System;
using System.Collections.Generic;
using System.Linq;

using Bong.Core.Domain.Goods;

namespace Bong.Services.Goods
{
    public static class CategoryExtension
    {
        public static IList<Category> SortCategoriesForTree(this IList<Category> source, int parentId = 0, bool ignoreCategoriesWithoutExistingParent = false)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var result = new List<Category>();

            foreach (var cat in source.ToList().FindAll(c => c.ParentCategoryId == parentId))
            {
                result.Add(cat);
                result.AddRange(SortCategoriesForTree(source, cat.Id, true));
            }
            if (!ignoreCategoriesWithoutExistingParent && result.Count != source.Count)
            {
                //find categories without parent in provided category source and insert them into result
                foreach (var cat in source)
                    if (result.FirstOrDefault(x => x.Id == cat.Id) == null)
                        result.Add(cat);
            }
            return result;
        }

        public static IList<Category> GetCategoryBreadCrumb(this Category category, ICategoryService categoryService)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            var result = new List<Category>();

            //used to prevent circular references
            while (category != null && !category.Deleted)
            {
                result.Add(category);
                category = categoryService.GetCategoryById(category.ParentCategoryId);
            }
            result.Reverse();
            return result;
        }
    }
}
