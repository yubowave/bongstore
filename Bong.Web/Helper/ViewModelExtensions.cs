using System;
using System.Collections.Generic;
using System.Linq;

using Bong.Core.Configuration;
using Bong.Core.Domain.Goods;
using Bong.Services.Goods;
using Bong.Core.Domain.Media;
using Bong.Services.Media;
using Bong.Web.Models;

namespace Bong.Web.Helper
{
    public static class ViewModelExtensions
    {
        #region Products

        /// <summary>
        /// 
        /// </summary>
        /// <param name="products"></param>
        /// <param name="pictureService"></param>
        /// <param name="sysSetting"></param>
        /// <param name="pictureSize">if null, don't load picture</param>
        /// <returns></returns>
        public static IList<ProductViewModel> PrepareProductViewModels(this IList<Product> products, IPictureService pictureService,
            SystemSetting sysSetting, int? pictureSize = null)
        {
            IList<ProductViewModel> results = new List<ProductViewModel>();

            foreach (var product in products)
            {
                ProductViewModel model = new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    ShortDescription = product.ShortDescription,
                    Description = product.ShortDescription,
                    Price = product.Price,
                    RatingSum = product.RatingSum,
                    TotalReviews = product.TotalReviews
                };

                if (pictureSize.HasValue && pictureService != null && sysSetting != null)
                {
                    //picture
                    var picture = pictureService.GetPicturesByProductId(product.Id, 1).FirstOrDefault();
                    int imageSize = pictureSize > 0 ? pictureSize.Value : sysSetting.DefaultThumbPicSize;

                    // PictureModel
                    PictureModel pictureModel = new PictureModel
                    {
                        ImageUrl = pictureService.GetPictureUrl(picture, imageSize),
                        FullSizeImageUrl = pictureService.GetPictureUrl(picture),
                        Title = string.Format("Show detail for {0}", product.Name),
                        AlternateText = string.Format("Image of {0}", product.Name)
                    };
                    model.PictureModel = pictureModel;
                }
                results.Add(model);
            }
            return results;
        }

        #endregion

        #region Category

        public static IList<CategoryView> PrepareCategoryViewModels(this IList<Category> categories, 
            IPictureService pictureService, SystemSetting sysSetting, int? pictureSize = null)
        {
            IList<CategoryView> results = new List<CategoryView>();

            foreach (var item in categories)
            {
                CategoryView view = new CategoryView
                {
                    Id = item.Id,
                    Name = item.Name
                };

                // Prepare the picture model
                if (pictureSize.HasValue && pictureService != null && sysSetting != null)
                {
                    //picture
                    var picture = pictureService.GetPicturesByProductId(item.Id, 1).FirstOrDefault();
                    int imageSize = pictureSize > 0 ? pictureSize.Value : sysSetting.DefaultThumbPicSize;

                    // PictureModel
                    PictureModel pictureModel = new PictureModel
                    {
                        ImageUrl = pictureService.GetPictureUrl(picture, imageSize),
                        FullSizeImageUrl = pictureService.GetPictureUrl(picture),
                        Title = string.Format("Show detail for {0}", item.Name),
                        AlternateText = string.Format("Image of {0}", item.Name)
                    };
                    view.PictureModel = pictureModel;
                }
                results.Add(view);
            }
            return results;
        }

        #endregion
    }
}