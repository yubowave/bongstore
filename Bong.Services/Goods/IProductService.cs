using System;
using System.Collections.Generic;
using Bong.Core.Collections;
using Bong.Core.Domain.Goods;

namespace Bong.Services.Goods
{
    public partial interface IProductService
    {
        #region Products

        IList<Product> GetAllProductsDisplayedOnHomePage();

        Product GetProductById(int productId);

        IList<Product> GetProductsByIds(int[] productIds);

        void InsertProduct(Product product);

        void UpdateProduct(Product product);

        void DeleteProduct(Product product);

        IPagedList<Product> SearchProducts(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<int> categoryIds = null,
            bool? featuredProducts = null,
            decimal? priceMin = null,
            decimal? priceMax = null,
            string keywords = null,
            bool searchDescriptions = false,
            ProductSortingEnum orderBy = ProductSortingEnum.Position);

        void UpdateProductReviewTotals(Product product);

        #endregion

        #region Product pictures

        IList<ProductPicture> GetProductPicturesByProductId(int productId);

        ProductPicture GetProductPictureById(int productPictureId);

        void InsertProductPicture(ProductPicture productPicture);

        void UpdateProductPicture(ProductPicture productPicture);

        void DeleteProductPicture(ProductPicture productPicture);

        #endregion

        #region Product reviews

        IList<ProductReview> GetAllProductReviews(int customerId, DateTime? fromUtc = null, 
            DateTime? toUtc = null, string message = null);

        ProductReview GetProductReviewById(int productReviewId);

        void DeleteProductReview(ProductReview productReview);

        #endregion
    }
}
