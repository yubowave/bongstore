using System.Linq;
using Bong.Core.Domain;

namespace Bong.Core.Data
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
    }
}
