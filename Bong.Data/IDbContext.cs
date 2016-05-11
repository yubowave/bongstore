using System;
using System.Collections.Generic;
using System.Data.Entity;
using Bong.Core.Domain;

namespace Bong.Data
{
    public interface IDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        int SaveChanges();

        IList<TEntity> ExecuteStoredProcedure<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new();

        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);
    }
}
