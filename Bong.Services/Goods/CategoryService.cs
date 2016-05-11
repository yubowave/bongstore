using System;
using System.Collections.Generic;
using System.Linq;

using Bong.Core.Collections;
using Bong.Core.Data;
using Bong.Core.Domain.Goods;
using Bong.Data;

namespace Bong.Services.Goods
{
    public class CategoryService : ICategoryService
    {
        #region Fields

        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;

        #endregion

        #region Ctor

        public CategoryService(IRepository<Category> categoryRepository, IRepository<Product> productRepository,
            IRepository<ProductCategory> productCategoryRepository)
        {
            this._categoryRepository = categoryRepository;
            this._productRepository = productRepository;
            this._productCategoryRepository = productCategoryRepository;
        }

        #endregion

        #region Methods

        public virtual IPagedList<Category> GetAllCategories(string categoryName = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _categoryRepository.Table;

            if (!String.IsNullOrWhiteSpace(categoryName))
                query = query.Where(c => c.Name.Contains(categoryName));
            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.ParentCategoryId);

            var unsortedCategories = query.ToList();

            //sort categories
            var sortedCategories = unsortedCategories.SortCategoriesForTree();

            //paging
            return new PagedList<Category>(sortedCategories, pageIndex, pageSize);
        }

        public virtual IList<Category> GetAllCategoriesByParentCategoryId(int? parentCategoryId)
        {
            var query = _categoryRepository.Table.Where(c => c.ParentCategoryId == parentCategoryId && !c.Deleted);

            return query.ToList();
        }

        public virtual IList<Category> GetAllCategoriesDisplayedOnHomePage()
        {
            var query = _categoryRepository.Table.Where(c => !c.Deleted && c.ShowOnHomePage);
            return query.ToList();
        }

        public virtual Category GetCategoryById(int? categoryId)
        {
            if (!categoryId.HasValue || categoryId == 0)
                return null;
            
            return _categoryRepository.GetById(categoryId.Value);
        }

        public virtual void InsertCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            _categoryRepository.Insert(category);
        }

        public virtual void UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            _categoryRepository.Update(category);
        }

        public virtual void DeleteCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            category.Deleted = true;
            UpdateCategory(category);

            //reset a "Parent category" property of all child subcategories
            var subcategories = GetAllCategoriesByParentCategoryId(category.Id);
            foreach (var subcategory in subcategories)
            {
                subcategory.ParentCategoryId = null;
                UpdateCategory(subcategory);
            }
        }

        public virtual IPagedList<ProductCategory> GetProductCategoriesByCategoryId(int categoryId, int pageIndex, int pageSize)
        {
            if (categoryId == 0)
                return new PagedList<ProductCategory>(new List<ProductCategory>(), pageIndex, pageSize);

            var query = from pc in _productCategoryRepository.Table
                        join p in _productRepository.Table on pc.ProductId equals p.Id
                        where pc.CategoryId == categoryId && !p.Deleted
                        orderby pc.ShowOrder
                        select pc;

            var productCategories = new PagedList<ProductCategory>(query, pageIndex, pageSize);
            return productCategories;
        }

        public virtual IList<ProductCategory> GetProductCategoriesByProductId(int productId)
        {
            if (productId == 0)
                return new List<ProductCategory>();

            var query = from pc in _productCategoryRepository.Table
                        join c in _categoryRepository.Table on pc.CategoryId equals c.Id
                        where pc.ProductId == productId && !c.Deleted
                        orderby pc.ShowOrder
                        select pc;

            return query.ToList();
        }

        public virtual ProductCategory GetProductCategoryById(int productCategoryId)
        {
            if (productCategoryId == 0)
                return null;

            return _productCategoryRepository.GetById(productCategoryId);
        }

        public virtual void InsertProductCategory(ProductCategory productCategory)
        {
            if (productCategory == null)
                throw new ArgumentNullException("productCategory");

            _productCategoryRepository.Insert(productCategory);
        }

        public virtual void UpdateProductCategory(ProductCategory productCategory)
        {
            if (productCategory == null)
                throw new ArgumentNullException("productCategory");

            _productCategoryRepository.Update(productCategory);
        }

        public virtual void DeleteProductCategory(ProductCategory productCategory)
        {
            if (productCategory == null)
                throw new ArgumentNullException("productCategory");

            _productCategoryRepository.Delete(productCategory);
        }
        
        #endregion
    }
}
