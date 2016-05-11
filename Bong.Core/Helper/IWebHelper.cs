using System.Web;

namespace Bong.Core.Helper
{
    public partial interface IWebHelper
    {
        string GetUrlReferrer();

        string GetCurrentIpAddress();

        string GetThisPageUrl(bool includeQueryString);

        bool IsCurrentConnectionSecured();

        string ServerVariables(string name);

        bool IsStaticResource(HttpRequest request);

        string MapPath(string path);

        string GetStoreHost(bool useSsl);

        string GetStoreLocation();

        string GetStoreLocation(bool useSsl);
    }
}
