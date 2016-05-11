using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ImageResizer;

using Bong.Core.Collections;
using Bong.Core.Helper;
using Bong.Core.Data;
using Bong.Core.Domain.Goods;
using Bong.Core.Domain.Media;

namespace Bong.Services.Media
{
    public partial class PictureService : IPictureService
    {
        #region Fields

        private static readonly object s_lock = new object();

        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public PictureService(IRepository<Picture> pictureRepository,
            IRepository<ProductPicture> productPictureRepository,
            IWebHelper webHelper)
        {
            this._pictureRepository = pictureRepository;
            this._productPictureRepository = productPictureRepository;
            this._webHelper = webHelper;
        }

        #endregion

        #region Utilities

        protected virtual string GetThumbsDirectory()
        {
            string path = _webHelper.MapPath("~/content/images/thumbs/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        protected virtual string GetImagesDirectory()
        {
            string path = _webHelper.MapPath("~/content/images/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        protected virtual Size CalculateDimensions(Size originalSize, int targetSize, bool ensureSizePositive = true)
        {
            var newSize = new Size();
            if (originalSize.Height > originalSize.Width)
            {
                // portrait 
                newSize.Width = (int)(originalSize.Width * (float)(targetSize / (float)originalSize.Height));
                newSize.Height = targetSize;
            }
            else
            {
                // landscape or square
                newSize.Height = (int)(originalSize.Height * (float)(targetSize / (float)originalSize.Width));
                newSize.Width = targetSize;
            }

            if (ensureSizePositive)
            {
                if (newSize.Width < 1) newSize.Width = 1;
                if (newSize.Height < 1) newSize.Height = 1;
            }
            return newSize;
        }

        protected virtual string GetFileExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
                return null;

            string[] parts = mimeType.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            return lastPart;
        }

        protected virtual byte[] LoadPictureFromFile(int pictureId, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = string.Format("{0}_0.{1}", pictureId.ToString("0000000"), lastPart);
            var filePath = GetPictureLocalPath(fileName);
            if (!File.Exists(filePath))
                return new byte[0];
            return File.ReadAllBytes(filePath);
        }

        protected virtual void SavePictureInFile(int pictureId, byte[] pictureBinary, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = string.Format("{0}_0.{1}", pictureId.ToString("0000000"), lastPart);
            File.WriteAllBytes(GetPictureLocalPath(fileName), pictureBinary);
        }

        protected virtual void DeletePictureOnFileSystem(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            string lastPart = GetFileExtensionFromMimeType(picture.Mime);
            string fileName = string.Format("{0}_0.{1}", picture.Id.ToString("0000000"), lastPart);
            string filePath = GetPictureLocalPath(fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        protected virtual void DeletePictureThumbs(Picture picture)
        {
            string filter = string.Format("{0}*.*", picture.Id.ToString("0000000"));
            var thumbDirectoryPath = GetThumbsDirectory();
            string[] currentFiles = System.IO.Directory.GetFiles(thumbDirectoryPath, filter, SearchOption.AllDirectories);
            foreach (string currentFileName in currentFiles)
            {
                var thumbFilePath = GetThumbLocalPath(currentFileName);
                if (File.Exists(thumbFilePath))
                    File.Delete(thumbFilePath);
            }
        }

        protected virtual string GetThumbLocalPath(string thumbFileName)
        {
            var thumbsDirectoryPath = GetThumbsDirectory();
            var thumbFilePath = Path.Combine(thumbsDirectoryPath, thumbFileName);
            return thumbFilePath;
        }

        protected virtual string GetThumbUrl(string thumbFileName)
        {
            string storeLocation = _webHelper.GetStoreLocation();
            return storeLocation + "content/images/thumbs/" + thumbFileName;
        }

        protected virtual string GetPictureLocalPath(string fileName)
        {
            var imagesDirectoryPath = GetImagesDirectory();
            var filePath = Path.Combine(imagesDirectoryPath, fileName);
            return filePath;
        }

        #endregion

        #region Getting picture local path/URL methods

        public virtual byte[] LoadPictureBinary(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            return picture.PictureBinary;
        }

        public virtual string GetDefaultPictureUrl(int targetSize = 0)
        {
            string defaultImageFileName = "default.gif";
            string filePath = GetPictureLocalPath(defaultImageFileName);

            if (!File.Exists(filePath)) return "";

            if (targetSize == 0)
            {
                string url = _webHelper.GetStoreLocation() + "content/images/" + defaultImageFileName;
                return url;
            }
            else
            {
                string fileExtension = Path.GetExtension(filePath);
                string thumbFileName = string.Format("{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(filePath),
                    targetSize,
                    fileExtension);
                var thumbFilePath = GetThumbLocalPath(thumbFileName);
                if (!File.Exists(thumbFilePath))
                {
                    using (var b = new Bitmap(filePath))
                    {
                        var newSize = CalculateDimensions(b.Size, targetSize);

                        var destStream = new MemoryStream();
                        ImageBuilder.Current.Build(b, destStream, new ResizeSettings()
                        {
                            Width = newSize.Width,
                            Height = newSize.Height,
                            Scale = ScaleMode.Both
                        });
                        var destBinary = destStream.ToArray();
                        File.WriteAllBytes(thumbFilePath, destBinary);
                    }
                }
                var url = GetThumbUrl(thumbFileName);
                return url;
            }
        }

        public virtual string GetPictureUrl(int pictureId, int targetSize = 0, bool showDefaultPicture = true)
        {
            var picture = GetPictureById(pictureId);
            return GetPictureUrl(picture, targetSize, showDefaultPicture);
        }

        public virtual string GetPictureUrl(Picture picture, int targetSize = 0, bool showDefaultPicture = true)
        {
            string url = string.Empty;
            byte[] pictureBinary = null;
            if (picture != null)
                pictureBinary = LoadPictureBinary(picture);
            if (picture == null || pictureBinary == null || pictureBinary.Length == 0)
            {
                if (showDefaultPicture)
                {
                    url = GetDefaultPictureUrl(targetSize);
                }
                return url;
            }

            string lastPart = GetFileExtensionFromMimeType(picture.Mime);
            string thumbFileName;
            if (picture.IsNew)
            {
                DeletePictureThumbs(picture);
                picture = UpdatePicture(picture.Id, pictureBinary, picture.Mime, false);
            }
            lock (s_lock)
            {
                if (targetSize == 0)
                {
                    thumbFileName = string.Format("{0}.{1}", picture.Id.ToString("0000000"), lastPart);
                    var thumbFilePath = GetThumbLocalPath(thumbFileName);
                    if (!File.Exists(thumbFilePath))
                    {
                        File.WriteAllBytes(thumbFilePath, pictureBinary);
                    }
                }
                else
                {
                    thumbFileName = string.Format("{0}_{1}.{2}", picture.Id.ToString("0000000"), targetSize, lastPart);
                    var thumbFilePath = GetThumbLocalPath(thumbFileName);
                    if (!File.Exists(thumbFilePath))
                    {
                        using (var stream = new MemoryStream(pictureBinary))
                        {
                            Bitmap b = null;
                            try
                            {
                                b = new Bitmap(stream);
                            }
                            catch (ArgumentException)
                            {
                                // Logger.Error
                            }
                            if (b == null)
                            {
                                //bitmap could not be loaded for some reasons
                                return url;
                            }

                            var newSize = CalculateDimensions(b.Size, targetSize);

                            var destStream = new MemoryStream();
                            ImageBuilder.Current.Build(b, destStream, new ResizeSettings()
                            {
                                Width = newSize.Width,
                                Height = newSize.Height,
                                Scale = ScaleMode.Both
                            });
                            var destBinary = destStream.ToArray();
                            File.WriteAllBytes(thumbFilePath, destBinary);

                            b.Dispose();
                        }
                    }
                }
            }
            url = GetThumbUrl(thumbFileName);
            return url;
        }

        public virtual string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true)
        {
            string url = GetPictureUrl(picture, targetSize, showDefaultPicture);
            if (String.IsNullOrEmpty(url))
                return String.Empty;
            else
                return GetThumbLocalPath(Path.GetFileName(url));
        }

        #endregion

        #region CRUD methods

        public virtual Picture GetPictureById(int pictureId)
        {
            if (pictureId == 0)
                return null;

            return _pictureRepository.GetById(pictureId);
        }

        public virtual void DeletePicture(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            //delete thumbs
            DeletePictureThumbs(picture);

            //delete from database
            _pictureRepository.Delete(picture);
        }

        public virtual IPagedList<Picture> GetPictures(int pageIndex, int pageSize)
        {
            var query = from p in _pictureRepository.Table
                        orderby p.Id descending
                        select p;
            var pics = new PagedList<Picture>(query, pageIndex, pageSize);
            return pics;
        }

        public virtual IList<Picture> GetPicturesByProductId(int productId, int recordsToReturn = 0)
        {
            if (productId == 0)
                return new List<Picture>();

            var query = from p in _pictureRepository.Table
                        join pp in _productPictureRepository.Table on p.Id equals pp.PictureId
                        orderby pp.ShowOrder
                        where pp.ProductId == productId
                        select p;

            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);

            var pics = query.ToList();
            return pics;
        }

        public virtual Picture InsertPicture(byte[] pictureBinary, string mime, bool isNew)
        {
            mime = CommonHelper.EnsureNotNull(mime);
            mime = CommonHelper.EnsureMaximumLength(mime, 20);

            var picture = new Picture() { PictureBinary = pictureBinary,Mime = mime, IsNew = isNew };
            _pictureRepository.Insert(picture);
            return picture;
        }

        public virtual Picture UpdatePicture(int pictureId, byte[] pictureBinary, string mime, bool isNew)
        {
            mime = CommonHelper.EnsureNotNull(mime);
            mime = CommonHelper.EnsureMaximumLength(mime, 20);

            var picture = GetPictureById(pictureId);
            if (picture == null)
                return null;

            picture.PictureBinary = pictureBinary;
            picture.Mime = mime;
            picture.IsNew = isNew;

            _pictureRepository.Update(picture);
            return picture;
        }

        #endregion
    }
}