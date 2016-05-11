using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Reflection;

using Bong.Core.Domain;

namespace Bong.Data
{
    /// <summary>
    /// Database context
    /// </summary>
    public class BongDbContext : DbContext, IDbContext
    {
        #region ctors
        public BongDbContext(string nameOrConnString) : base(nameOrConnString)
        {

        }
        public BongDbContext() : base("BongConnection")
        {
        }
        #endregion

        #region Implementation of DbContext Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //dynamically load all configuration
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Implementation of IDbContext Methods

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
        
        public IList<TEntity> ExecuteStoredProcedure<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not Support parameter type");

                    commandText += i == 0 ? " " : ", "; 
                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                        commandText += " output";
                }
            }
            var results = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();

            return results;
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction ? 
                TransactionalBehavior.DoNotEnsureTransaction : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }
            return result;
        }

        #endregion
    }
}
