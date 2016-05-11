using System;
using System.Collections.Generic;
using System.Data.Entity;

using Bong.Data;
using Bong.Core;
using Bong.Core.Domain.Customers;
using Bong.Core.Domain.Goods;

namespace Bong.Data.Initializers
{
    public class BongDropCreateDBIfModelChanges : DropCreateDatabaseIfModelChanges<BongDbContext>
    {
        protected override void Seed(BongDbContext context)
        {
        }
    }
}
