using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Bong.Core.Configuration;
using Bong.Core.Domain.Goods;
using Bong.Core.Domain.Media;
using Bong.Services.Goods;
using Bong.Services.Media;
using Bong.Web.Models;
using Bong.Web.Helper;
using Bong.Web.Models.Parameter;

namespace Bong.Web.Controllers
{
    public class CategoryController : Controller
    {
        #region Fields

        private readonly ICategoryService _cateService;
        private readonly IProductService _productService;
        private readonly IPictureService _pictureService;
        private readonly SystemSetting _sysSetting;

        #endregion

        #region ctor

        public CategoryController(ICategoryService cateService,  IProductService productService,
            IPictureService pictureService, SystemSetting sysSetting)
        {
            _cateService = cateService;
            _pictureService = pictureService;
            _sysSetting = sysSetting;
            _productService = productService;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected virtual IList<CategoryView> PrepareCategoryViewsByPath(int? rootCategoryId, 
            IList<int> loadSubCategoriesInIds, bool isLoadPicture)
        {
            var result = new List<CategoryView>();
            foreach (var category in _cateService.GetAllCategoriesByParentCategoryId(rootCategoryId))
            {
                var categoryModel = new CategoryView()
                {
                    Id = category.Id,
                    Name = category.Name
                };

                if (isLoadPicture)
                {
                    //picture
                    var picture = _pictureService.GetPictureById(category.PictureId);
                    int imageSize = _sysSetting.DefaultGridPicSize;

                    // PictureModel
                    PictureModel pictureModel = new PictureModel
                    {
                        ImageUrl = _pictureService.GetPictureUrl(picture, imageSize),
                        FullSizeImageUrl = _pictureService.GetPictureUrl(picture),
                        Title = string.Format("Show detail for {0}", category.Name),
                        AlternateText = string.Format("Image of {0}", category.Name)
                    };
                    categoryModel.PictureModel = pictureModel;
                }

                //load subcategories?
                if (loadSubCategoriesInIds != null && loadSubCategoriesInIds.Contains(category.Id))
                {
                    var subCategories = PrepareCategoryViewsByPath(category.Id, loadSubCategoriesInIds, isLoadPicture);
                    categoryModel.SubCategories = subCategories;
                }
                result.Add(categoryModel);
            }
            return result;
        }

        #endregion

        #region Views

        public ActionResult CategoryDetails(int categoryId, CategoryPagingFilteringParam filters)
        {
            if (categoryId == 0)
                return Content("");

            var category = _cateService.GetCategoryById(categoryId);
            if (category == null)
                return Content("");

            CategoryDetailModel model = new CategoryDetailModel();
            var breadCrumbs = category.GetCategoryBreadCrumb(_cateService);
            model.CategoryBreadcrumb = breadCrumbs.PrepareCategoryViewModels(_pictureService, _sysSetting, null);
            // Category Detail
            model.Id = category.Id;
            model.Name = category.Name;

            // SubCategories
            model.SubCategories = PrepareCategoryViewsByPath(model.Id, null, true);

            // Products
            IList<int> categoryIds = new List<int>();
            categoryIds.Add(categoryId);
            var products = _productService.SearchProducts(0, int.MaxValue, new List<int> { model.Id });
            model.Products = products.PrepareProductViewModels(_pictureService, _sysSetting, _sysSetting.DefaultGridPicSize);
            
            return View(model);
        }

        #endregion
        
        #region Partial Views

        public ActionResult MainMenu()
        {
            IList<CategoryView> results = PrepareCategoryViewsByPath(null, null, false);

            foreach (var item in results)
            {
                item.SubCategories = PrepareCategoryViewsByPath(item.Id, null, false);
            }
            return PartialView(results);
        }

        public ActionResult NavCategeries(int? currentCategoryId = null, int? currentProductId = null)
        {
            // active category or active product
            int? activeCategoryId = null; ;
            if (currentCategoryId.HasValue && currentCategoryId > 0)
            {
                activeCategoryId = currentCategoryId.Value;
            }
            else if (currentProductId.HasValue && currentProductId > 0)
            {
                var productCategories = _cateService.GetProductCategoriesByProductId(currentProductId.Value);
                if (productCategories.Count > 0)
                    activeCategoryId = productCategories[0].CategoryId;
            }

            var activeCategory = _cateService.GetCategoryById(activeCategoryId);
            var breadCrumb = activeCategory != null ? 
                activeCategory.GetCategoryBreadCrumb(_cateService).Select(x => x.Id).ToList() : new List<int>();
            var categories = PrepareCategoryViewsByPath(null, breadCrumb, false).ToList();

            var model = new CategoryNavModel()
            {
                CurrentCategoryId = activeCategoryId,
                Categories = categories
            };

            return PartialView(model);
        }

        public ActionResult CategoriesOnHome()
        {
            IList<Category> results = _cateService.GetAllCategoriesDisplayedOnHomePage(); 
            return PartialView();
        }

        public ActionResult SearchPart()
        {
            return PartialView();
        }

        public ActionResult CategoriesHomePage()
        {
            return PartialView();
        }

        #endregion
    }
}