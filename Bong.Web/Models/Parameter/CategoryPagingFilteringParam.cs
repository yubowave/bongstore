using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bong.Web.Models.Parameter
{
    public class CategoryPagingFilteringParam
    {
        #region Properties

        public int FirstItem { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public int LastItem { get; set; }

        public int PageIndex
        {
            get
            {
                if (PageNumber > 0)
                    return PageNumber - 1;
                else
                    return 0;
            }
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        #endregion
    }
}