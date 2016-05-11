using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bong.Core.Domain.Goods; 

namespace Bong.Core.Domain.Media
{
    public class Picture : BaseEntity
    {
        public byte[] PictureBinary { get; set; }

        public string Mime { get; set; }

        public bool IsNew { get; set; }
    }
}
