using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace Bong.Core.Helper
{
    public partial class WebHelper : IWebHelper
    {
        #region Fields

        private readonly HttpContextBase _httpContext;

        #endregion

        #region ctor

        public WebHelper(HttpContextBase httpContext)
        {
            this._httpContext = httpContext;
        }

        #endregion

        #region Utilities

        protected virtual Boolean IsRequestAvailable(HttpContextBase httpContext)
        {
            if (httpContext == null)
                return false;

            try
            {
                if (httpContext.Request == null)
                    return false;
            }
            catch (HttpException)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Methods

        public virtual string GetUrlReferrer()
        {
            string referrerUrl = string.Empty;

            //URL referrer is null in some case (for example, in IE 8)
            if (IsRequestAvailable(_httpContext) && _httpContext.Request.UrlReferrer != null)
                referrerUrl = _httpContext.Request.UrlReferrer.PathAndQuery;

            return referrerUrl;
        }

        public virtual string GetCurrentIpAddress()
        {
            if (!IsRequestAvailable(_httpContext))
                return string.Empty;

            var result = "";
            if (_httpContext.Request.Headers != null)
            {
                var forwardedHttpHeader = "X-FORWARDED-FOR";
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ForwardedHTTPheader"]))
                {
                    forwardedHttpHeader = ConfigurationManager.AppSettings["ForwardedHTTPheader"];
                }
                string xff = _httpContext.Request.Headers.AllKeys
                    .Where(x => forwardedHttpHeader.Equals(x, StringComparison.InvariantCultureIgnoreCase))
                    .Select(k => _httpContext.Request.Headers[k])
                    .FirstOrDefault();

                if (!String.IsNullOrEmpty(xff))
                {
                    string lastIp = xff.Split(new char[] { ',' }).FirstOrDefault();
                    result = lastIp;
                }
            }

            if (String.IsNullOrEmpty(result) && _httpContext.Request.UserHostAddress != null)
            {
                result = _httpContext.Request.UserHostAddress;
            }

            //some validation
            if (result == "::1")
                result = "127.0.0.1";
            //remove port
            if (!String.IsNullOrEmpty(result))
            {
                int index = result.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                if (index > 0)
                    result = result.Substring(0, index);
            }
            return result;
        }

        public virtual string GetThisPageUrl(bool includeQueryString)
        {
            string url = string.Empty;
            if (!IsRequestAvailable(_httpContext))
                return url;

            if (includeQueryString)
            {
                if (_httpContext.Request.Url != null)
                {
                    url = _httpContext.Request.Url.GetLeftPart(UriPartial.Query);
                }
            }
            else
            {
                if (_httpContext.Request.Url != null)
                {
                    url = _httpContext.Request.Url.GetLeftPart(UriPartial.Path);
                }
            }
            
            url = url.ToLowerInvariant();
            return url;
        }

        public virtual bool IsCurrentConnectionSecured()
        {
            bool useSsl = false;
            if (IsRequestAvailable(_httpContext))
            {
                useSsl = _httpContext.Request.IsSecureConnection;
            }
            return useSsl;
        }

        public virtual string ServerVariables(string name)
        {
            string result = string.Empty;
            try
            {
                if (!IsRequestAvailable(_httpContext))
                    return result;

                if (_httpContext.Request.ServerVariables[name] != null)
                {
                    result = _httpContext.Request.ServerVariables[name];
                }
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

        public virtual bool IsStaticResource(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            string path = request.Path;
            string extension = VirtualPathUtility.GetExtension(path);

            if (extension == null) return false;

            switch (extension.ToLower())
            {
                case ".axd":
                case ".ashx":
                case ".bmp":
                case ".css":
                case ".gif":
                case ".htm":
                case ".html":
                case ".ico":
                case ".jpeg":
                case ".jpg":
                case ".js":
                case ".png":
                case ".rar":
                case ".zip":
                    return true;
            }

            return false;
        }

        public virtual string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                return HostingEnvironment.MapPath(path);
            }
            else
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
                return Path.Combine(baseDirectory, path);
            }
        }

        public virtual bool IsRequestBeingRedirected
        {
            get
            {
                return _httpContext.Response.IsRequestBeingRedirected;
            }
        }

        public virtual string GetStoreHost(bool useSsl)
        {
            var result = string.Empty;
            if (useSsl)
                result = "https://";
            else
                result = "http://";

            var httpHost = ServerVariables("HTTP_HOST");
            if (!String.IsNullOrEmpty(httpHost))
                result += httpHost;

            if (!result.EndsWith("/"))
                result += "/";
            return result.ToLowerInvariant();
        }

        public virtual string GetStoreLocation()
        {
            bool useSsl = IsCurrentConnectionSecured();
            return GetStoreLocation(useSsl);
        }

        public virtual string GetStoreLocation(bool useSsl)
        {
            string result = GetStoreHost(useSsl);
            if (result.EndsWith("/"))
                result = result.Substring(0, result.Length - 1);
            if (IsRequestAvailable(_httpContext))
                result = result + _httpContext.Request.ApplicationPath;
            if (!result.EndsWith("/"))
                result += "/";

            return result.ToLowerInvariant();
        }

        #endregion
    }
}
