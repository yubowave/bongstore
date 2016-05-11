using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Bong.Core.Domain;
using Bong.Core.Data;

namespace Bong.Data
{
    public partial class EFRepositoty<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IDbContext _context;
        private IDbSet<T> _entities;

        public EFRepositoty(IDbContext context)
        {
            _context = context;
        }

        protected IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }
                return _entities;
            }
        }

        public T GetById(int id)
        {
            return this.Entities.Find(id);
        }
        public void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                
                this.Entities.Add(entity);
                _context.SaveChanges();
            }
            catch(DbEntityValidationException e)
            {
                var msg = string.Empty;
                foreach (var errors in e.EntityValidationErrors)
                {
                    foreach (var error in errors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
                }
                throw new Exception(msg, e);
            }
        }
        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var msg = string.Empty;
                foreach (var errors in e.EntityValidationErrors)
                {
                    foreach (var error in errors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
                }
                throw new Exception(msg, e);
            }
        }
        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);
                this._context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var msg = string.Empty;
                foreach (var errors in e.EntityValidationErrors)
                {
                    foreach (var error in errors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
                }
                throw new Exception(msg, e);
            }
        }
        public void Delete(int id)
        {
            try
            {
                var entity = this.Entities.Create();
                entity.Id = id;
                this.Entities.Attach(entity);
                this.Entities.Remove(entity);
                this._context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var msg = string.Empty;
                foreach (var errors in e.EntityValidationErrors)
                {
                    foreach (var error in errors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
                }
                throw new Exception(msg, e);
            }
        }
        public IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }
        public IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }
    }
}
 