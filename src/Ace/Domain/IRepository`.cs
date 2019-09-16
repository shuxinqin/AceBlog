using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ace.Domain
{
    public interface IRepository<TEntity> : IRepository
    {
        IQuery<TEntity> Query();
        IQuery<TEntity> QueryWithTracking();
        TEntity Get(object id, bool tracking = false);
        TEntity Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Track(TEntity entity);
    }
}
