using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IQuery<TEntity> Query()
        {
            return this.DbContext.Query<TEntity>();
        }

        public IQuery<TEntity> QueryWithTracking()
        {
            return this.DbContext.Query<TEntity>().AsTracking();
        }

        public TEntity Get(object id, bool tracking = false)
        {
            return this.DbContext.QueryByKey<TEntity>(id, tracking);
        }
        public async Task<TEntity> GetAsync(object id, bool tracking = false)
        {
            return await this.DbContext.QueryByKeyAsync<TEntity>(id, tracking);
        }

        public TEntity Insert(TEntity entity)
        {
            return this.DbContext.Insert(entity);
        }
        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            return await this.DbContext.InsertAsync(entity);
        }

        public void Update(TEntity entity)
        {
            this.DbContext.Update(entity);
        }
        public async Task UpdateAsync(TEntity entity)
        {
            await this.DbContext.UpdateAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            this.DbContext.Delete(entity);
        }
        public async Task DeleteAsync(TEntity entity)
        {
            await this.DbContext.DeleteAsync(entity);
        }

        public void Track(TEntity entity)
        {
            this.DbContext.TrackEntity(entity);
        }
    }
}
