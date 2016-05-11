using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Bong.Core.Configuration;
using Bong.Core.Domain.Goods;
using Bong.Services.Goods;
using Bong.Core.Domain.Media;
using Bong.Services.Media;
using Bong.Web.Models;
using Bong.Web.Helper;

namespace Bong.Web.Controllers
{
    public class ProductController : Controller
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IViewedProductService _viewdProductService;
        private readonly IPictureService _pictureService;
        private readonly SystemSetting _sysSetting;

        #endregion

        #region ctor

        public ProductController(ICategoryService categoryService ,IProductService productService, 
            SystemSetting sysSetting, IViewedProductService viewdProductService, IPictureService pictureService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _viewdProductService = viewdProductService;
            _pictureService = pictureService;
            _sysSetting = sysSetting;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected virtual ProductDetailModel PrepareProductDetailModel(Product product)
        {
            if (product == null)
                return null;

            ProductDetailModel result = new ProductDetailModel
            {
                Id = product.Id,
                Name = product.Name,
                ShortDescription=product.ShortDescription,
                Description = product.Description,
                Price=product.Price,
                OldPrice=product.OldPrice,
                Weight=product.Weight,
                Length=product.Length,
                Width=product.Width,
                Height=product.Height,
                ShowOnHomePage=product.ShowOnHomePage,
                CreatedOnUtc=product.CreatedOnUtc,
                UpdatedOnUtc=product.UpdatedOnUtc,
                RatingSum=product.RatingSum,
                TotalReviews=product.TotalReviews,
            };

            // Pictures
            var pictures = _pictureService.GetPicturesByProductId(product.Id);
            if (pictures.Count > 0)
            {
                var firestPicture = pictures.FirstOrDefault();
                // Default picture model
                result.DefaultPictureModel = new PictureModel
                {
                    ImageUrl = _pictureService.GetPictureUrl(firestPicture, _sysSetting.ProductDefaultPicSize),
                    FullSizeImageUrl = _pictureService.GetPictureUrl(firestPicture),
                    Title = string.Format("Show detail for {0}", product.Name),
                    AlternateText = string.Format("Image of {0}", product.Name)
                };

                // all picture models
                var pictureModels = new List<PictureModel>();
                foreach (var picture in pictures)
                {
                    result.PictureModels.Add(new PictureModel() 
                    {
                        ImageUrl = _pictureService.GetPictureUrl(picture, _sysSetting.ProductDetailPicSize),
                        FullSizeImageUrl = _pictureService.GetPictureUrl(picture),
                        Title = string.Format("Show detail for {0}", product.Name),
                        AlternateText = string.Format("Image of {0}", product.Name)
                    });
                }
            }

            // CategoryBreadcrumb
            var categories = _categoryService.GetProductCategoriesByProductId(product.Id);
            if (categories.Count > 0)
            {
                var category = categories[0].Category;
                if (category != null)
                {
                    var breadCrumbs = category.GetCategoryBreadCrumb(_categoryService);
                    result.CategoryBreadcrumb = breadCrumbs.PrepareCategoryViewModels(_pictureService, _sysSetting, null);
                }
            }

            return result;
        }

        #endregion

        #region Views

        public ActionResult ProductDetails(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null || product.Deleted)
                return RedirectToRoute("NoPageFound");

            // Product details - needed a view model?
            var model = PrepareProductDetailModel(product);
            
            // record as recently visited
            _viewdProductService.AddViewdProduct(productId);

            return View(model);
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult RecentViewedProducts()
        {
            var products = _viewdProductService.GetRecentViewedProducts(_sysSetting.ShowRecentProductNumber);

            //prepare model
            var model = new SimpleProductList();
            model.Name = "Recently Viewed Products";
            model.ProductViews = products.PrepareProductViewModels(_pictureService, _sysSetting, _sysSetting.DefaultGridPicSize);

            return View("SimpleProductList", model);
        }

        public ActionResult LatestAddedProducts()
        {
            var products = _productService.SearchProducts(0, 8, null, null, null, null, null, false, ProductSortingEnum.CreatedOn);

            var model = new SimpleProductList();
            model.Name = "New Products";
            model.ProductViews = products.PrepareProductViewModels(_pictureService, _sysSetting, _sysSetting.DefaultGridPicSize);

            return View("SimpleProductList", model);
        }

        #endregion

        #region Partial Views

        public ActionResult LatestViewProductsPart(int? showPicSize)
        {
            var products = _viewdProductService.GetRecentViewedProducts(_sysSetting.ShowRecentProductNumber);

            //prepare model
            var model = new List<ProductViewModel>();
            model.AddRange(products.PrepareProductViewModels(_pictureService, _sysSetting, showPicSize));

            return PartialView(model);
        }

        public ActionResult ProductsHomePage()
        {
            var products = _productService.GetAllProductsDisplayedOnHomePage();

            var model = products.PrepareProductViewModels(_pictureService, _sysSetting, _sysSetting.DefaultGridPicSize);

            return PartialView(model);
        }

        #endregion
    }
}