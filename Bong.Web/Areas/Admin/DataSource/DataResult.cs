using System.Collections;

namespace Bong.Web.Areas.Admin.DataSource
{
    public class DataResult
    {
        public IEnumerable Data { get; set; }

        public int Total { get; set; }

        public object Errors { get; set; }
    }
}