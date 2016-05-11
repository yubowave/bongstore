using System;
using System.Collections.Generic;
using System.Linq;

using Bong.Core.Data;
using Bong.Core.Collections;
using Bong.Core.Domain.Goods;

namespace Bong.Services.Goods
{
    public partial class ProductService : IProductService
    {
        #region Fields

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly IRepository<ProductReview> _productReviewRepository;

        #endregion

        #region Ctor

        public ProductService(IRepository<Product> productRepository,
            IRepository<ProductPicture> productPictureRepository, // IDbContext dbContext,
            IRepository<ProductReview> productReviewRepository)
        {
            this._productRepository = productRepository;
            this._productPictureRepository = productPictureRepository;
            this._productReviewRepository = productReviewRepository;
        }

        #endregion

        #region Methods

        #region Products

        public virtual IList<Product> GetAllProductsDisplayedOnHomePage()
        {
            var query = from p in _productRepository.Table
                        orderby p.Name
                        where !p.Deleted && p.ShowOnHomePage
                        select p;
            var products = query.ToList();
            return products;
        }

        public virtual Product GetProductById(int productId)
        {
            if (productId == 0)
                return null;

            return _productRepository.GetById(productId);
        }

        public virtual IList<Product> GetProductsByIds(int[] productIds)
        {
            if (productIds == null || productIds.Length == 0)
                return new List<Product>();

            var query = from p in _productRepository.Table
                        where productIds.Contains(p.Id)
                        select p;
            var products = query.ToList();

            //sort by passed identifiers
            var sortedProducts = new List<Product>();
            foreach (int id in productIds)
            {
                var product = products.Find(x => x.Id == id);
                if (product != null)
                    sortedProducts.Add(product);
            }
            return sortedProducts;
        }

        public virtual void InsertProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            //insert
            _productRepository.Insert(product);
        }

        public virtual void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            //update
            _productRepository.Update(product);
        }

        public virtual void DeleteProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            product.Deleted = true;
            //delete product
            UpdateProduct(product);
        }

        public virtual IPagedList<Product> SearchProducts(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<int> categoryIds = null,
            bool? featuredProducts = null,
            decimal? priceMin = null,
            decimal? priceMax = null,
            string keywords = null,
            bool searchDescriptions = false,
            ProductSortingEnum orderBy = ProductSortingEnum.Position)
        {

            //products
            var query = _productRepository.Table;
            query = query.Where(p => !p.Deleted);

            // price term
            var nowUtc = DateTime.UtcNow;
            if (priceMin.HasValue)
            {
                //min price
                query = query.Where(p => p.Price >= priceMin.Value );
            }
            if (priceMax.HasValue)
            {
                //max price
                query = query.Where(p => p.Price <= priceMax.Value);
            }
            
            //searching by keyword
            if (!String.IsNullOrWhiteSpace(keywords))
            {
                query = from p in query
                        where (p.Name.Contains(keywords)) ||
                              (searchDescriptions && p.ShortDescription.Contains(keywords)) ||
                              (searchDescriptions && p.Description.Contains(keywords)) 
                        select p;
            }
            //category filtering
            if (categoryIds != null && categoryIds.Count > 0)
            {
                query = from p in query
                        from pc in p.ProductCategories.Where(pc => categoryIds.Contains(pc.CategoryId))
                        where (!featuredProducts.HasValue || featuredProducts.Value == pc.IsFeaturedProduct)
                        select p;
            }
            //sort products
            if (orderBy == ProductSortingEnum.Position && categoryIds != null && categoryIds.Count > 0)
            {
                //category position
                var firstCategoryId = categoryIds[0];
                query = query.OrderBy(p => p.ProductCategories.FirstOrDefault(pc => pc.CategoryId == firstCategoryId).ShowOrder);
            }
            else if (orderBy == ProductSortingEnum.Position)
            {
                //otherwise sort by name
                query = query.OrderBy(p => p.Name);
            }
            else if (orderBy == ProductSortingEnum.NameAsc)
            {
                //Name: A to Z
                query = query.OrderBy(p => p.Name);
            }
            else if (orderBy == ProductSortingEnum.NameDesc)
            {
                //Name: Z to A
                query = query.OrderByDescending(p => p.Name);
            }
            else if (orderBy == ProductSortingEnum.PriceAsc)
            {
                //Price: Low to High
                query = query.OrderBy(p => p.Price);
            }
            else if (orderBy == ProductSortingEnum.PriceDesc)
            {
                //Price: High to Low
                query = query.OrderByDescending(p => p.Price);
            }
            else if (orderBy == ProductSortingEnum.CreatedOn)
            {
                //creation date
                query = query.OrderByDescending(p => p.CreatedOnUtc);
            }
            else
            {
                //actually this code is not reachable
                query = query.OrderBy(p => p.Name);
            }

            var products = new PagedList<Product>(query, pageIndex, pageSize);
            return products;
        }

        public virtual void UpdateProductReviewTotals(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            int ratingSum = 0;
            int totalReviews = 0;
            var reviews = product.ProductReviews;
            foreach (var pr in reviews)
            {
                ratingSum += pr.Rating;
                totalReviews++;
            }
            product.RatingSum = ratingSum;
            product.TotalReviews = totalReviews;
            UpdateProduct(product);
        }

        #endregion

        #region Product pictures

        public virtual IList<ProductPicture> GetProductPicturesByProductId(int productId)
        {
            var query = from pp in _productPictureRepository.Table
                        where pp.ProductId == productId
                        orderby pp.ShowOrder
                        select pp;
            var productPictures = query.ToList();
            return productPictures;
        }

        public virtual ProductPicture GetProductPictureById(int productPictureId)
        {
            if (productPictureId == 0)
                return null;

            return _productPictureRepository.GetById(productPictureId);
        }

        public virtual void InsertProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");

            _productPictureRepository.Insert(productPicture);
        }

        public virtual void UpdateProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");

            _productPictureRepository.Update(productPicture);
        }

        public virtual void DeleteProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");

            _productPictureRepository.Delete(productPicture);
        }


        #endregion

        #region Product reviews

        public virtual IList<ProductReview> GetAllProductReviews(int customerId, DateTime? fromUtc = null, 
            DateTime? toUtc = null, string message = null)
        {
            var query = _productReviewRepository.Table;
            if (customerId > 0)
                query = query.Where(c => c.CustomerId == customerId);
            if (fromUtc.HasValue)
                query = query.Where(c => fromUtc.Value <= c.CreatedOnUtc);
            if (toUtc.HasValue)
                query = query.Where(c => toUtc.Value >= c.CreatedOnUtc);
            if (!String.IsNullOrEmpty(message))
                query = query.Where(c => c.Title.Contains(message) || c.Content.Contains(message));

            query = query.OrderBy(c => c.CreatedOnUtc);
            return query.ToList();
        }

        public virtual ProductReview GetProductReviewById(int productReviewId)
        {
            if (productReviewId == 0)
                return null;

            return _productReviewRepository.GetById(productReviewId);
        }

        public virtual void DeleteProductReview(ProductReview productReview)
        {
            if (productReview == null)
                throw new ArgumentNullException("productReview");

            _productReviewRepository.Delete(productReview);
        }

        #endregion

        #endregion
    }
}
