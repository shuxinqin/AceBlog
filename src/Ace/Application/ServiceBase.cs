using Ace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ace.Application
{
    public abstract class ServiceBase
    {
        IUnitOfWork _uow;
        protected ServiceBase(IServiceProvider services) : this(services, null)
        {
        }
        protected ServiceBase(IServiceProvider services, IUnitOfWork uow)
        {
            this.Services = services;
            this._uow = uow;
        }

        public IServiceProvider Services { get; protected set; }

        public virtual IUnitOfWork Uow
        {
            get
            {
                if (this._uow == null)
                {
                    this._uow = this.Services.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
                }

                return this._uow;
            }
            set
            {
                this._uow = value;
            }
        }

        public virtual void UseTransaction(Action action)
        {
            this.Uow.BeginTransaction();
            try
            {
                action();
                this.Uow.CommitTransaction();
            }
            catch
            {
                this.Uow.RollbackTransaction();
                throw;
            }
        }
    }
}
