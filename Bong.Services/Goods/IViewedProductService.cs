using System.Collections.Generic;
using Bong.Core.Domain.Goods;

namespace Bong.Services.Goods
{
    public interface IViewedProductService
    {
        void AddViewdProduct(int productId);

        void ClearAll();

        IList<Product> GetRecentViewedProducts(int count = int.MaxValue);
    }
}
