using Chloe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ace.Domain
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void BeginTransaction(IsolationLevel il);
        void CommitTransaction();
        void RollbackTransaction();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public IDbContext DbContext { get; private set; }

        public void BeginTransaction()
        {
            this.DbContext.Session.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel il)
        {
            this.DbContext.Session.BeginTransaction(il);
        }

        public void CommitTransaction()
        {
            this.DbContext.Session.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            this.DbContext.Session.RollbackTransaction();
        }
    }
}
