using System.Collections.Generic;
using Bong.Core.Collections;
using Bong.Core.Domain.Goods;

namespace Bong.Services.Goods
{
    public interface ICategoryService
    {
        IPagedList<Category> GetAllCategories(string categoryName = "", int pageIndex = 0, int pageSize = int.MaxValue);

        IList<Category> GetAllCategoriesByParentCategoryId(int? parentCategoryId);

        IList<Category> GetAllCategoriesDisplayedOnHomePage();

        Category GetCategoryById(int? categoryId);

        void InsertCategory(Category category);

        void UpdateCategory(Category category);

        void DeleteCategory(Category category);

        IPagedList<ProductCategory> GetProductCategoriesByCategoryId(int categoryId, int pageIndex, int pageSize);

        IList<ProductCategory> GetProductCategoriesByProductId(int productId);

        ProductCategory GetProductCategoryById(int productCategoryId);

        void InsertProductCategory(ProductCategory productCategory);

        void UpdateProductCategory(ProductCategory productCategory);

        void DeleteProductCategory(ProductCategory productCategory);
    }
}
