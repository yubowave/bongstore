using System;
using System.Collections.Generic;
using System.Linq;

using Bong.Core.Collections;
using Bong.Core.Domain.Media;

namespace Bong.Services.Media
{
    public partial interface IPictureService
    {
        byte[] LoadPictureBinary(Picture picture);

        /// <summary>
        /// Gets the default picture URL
        /// </summary>
        string GetDefaultPictureUrl(int targetSize = 0);

        /// <summary>
        /// Get a picture URL
        /// </summary>
        string GetPictureUrl(int pictureId, 
            int targetSize = 0,
            bool showDefaultPicture = true);

        string GetPictureUrl(Picture picture, 
            int targetSize = 0,
            bool showDefaultPicture = true);

        /// <summary>
        /// Get a picture local path
        /// </summary>
        string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true);

        Picture GetPictureById(int pictureId);

        void DeletePicture(Picture picture);

        IPagedList<Picture> GetPictures(int pageIndex, int pageSize);

        IList<Picture> GetPicturesByProductId(int productId, int recordsToReturn = 0);

        Picture InsertPicture(byte[] pictureBinary, string mime, bool isNew);

        Picture UpdatePicture(int pictureId, byte[] pictureBinary, string mime, bool isNew);
    }
}
