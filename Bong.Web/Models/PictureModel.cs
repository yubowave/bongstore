using System;

namespace Bong.Web.Models
{
    public partial class PictureModel : BaseViewModel
    {
        public string ImageUrl { get; set; }

        public string FullSizeImageUrl { get; set; }

        public string Title { get; set; }

        public string AlternateText { get; set; }
    }
}