using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bong.Core.Configuration
{
    public class SystemSetting : ISettings
    {
        public int DefaultThumbPicSize { get { return 32; } }

        public int DefaultGridPicSize { get { return 125; } }

        public int ShowRecentProductNumber { get { return 5; } }

        public int ProductDefaultPicSize { get { return 300; } }

        public int ProductDetailPicSize { get { return 70; } }

        public int DefaultListPageSize { get { return 12; } }
    }
}
