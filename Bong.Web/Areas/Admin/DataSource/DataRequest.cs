namespace Bong.Web.Areas.Admin.DataSource
{
    public class DataRequest
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public DataRequest()
        {
            this.Page = 1;
            this.PageSize = 12;
        }
    }
}