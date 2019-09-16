using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chloe;

namespace Ace.Domain
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
    {
        protected RepositoryBase(IDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public IDbContext DbContext { get; private set; }

        public void Delete(TEntity entity)
        {
            this.DbContext.Delete(entity);
        }

        public TEntity Get(object id, bool tracking = false)
        {
            return this.DbContext.QueryByKey<TEntity>(id, tracking);
        }

        public TEntity Insert(TEntity entity)
        {
            return this.DbContext.Insert(entity);
        }

        public IQuery<TEntity> Query()
        {
            return this.DbContext.Query<TEntity>();
        }

        public IQuery<TEntity> QueryWithTracking()
        {
            return this.DbContext.Query<TEntity>().AsTracking();
        }

        public void Update(TEntity entity)
        {
            this.DbContext.Update(entity);
        }
        public void Track(TEntity entity)
        {
            this.DbContext.TrackEntity(entity);
        }
    }
}
