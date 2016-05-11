using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Bong.Data.Initializers;
using Bong.Core.Data;

namespace Bong.Data
{
    //public class SqlServerDbProvider : IDbProvider
    //{
    //    public void InitDatabase()
    //    {
    //        InitConnectionFactory();
    //        SetDatabaseInitializer();
    //    }

    //    public void InitConnectionFactory()
    //    {
    //        var connectionFactory = new SqlConnectionFactory();

    //        #pragma warning disable 0618
    //        Database.DefaultConnectionFactory = connectionFactory;
    //    }

    //    public void SetDatabaseInitializer()
    //    {
    //        var initializer = new BongDropCreateDBIfModelChanges();
    //        Database.SetInitializer(initializer);
    //    }
    //}
}
