using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Domain
{
    public interface IRepository<TEntity> : IRepository
    {
        IQuery<TEntity> Query();
        IQuery<TEntity> QueryWithTracking();

        TEntity Get(object id, bool tracking = false);
        Task<TEntity> GetAsync(object id, bool tracking = false);

        TEntity Insert(TEntity entity);
        Task<TEntity> InsertAsync(TEntity entity);

        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity);

        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);

        void Track(TEntity entity);
    }
}
