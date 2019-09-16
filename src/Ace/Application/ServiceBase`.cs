using Ace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ace.Application
{
    public abstract class ServiceBase<TRepository> : ServiceBase
    {
        TRepository _repository;
        protected ServiceBase(IServiceProvider services) : base(services)
        {
        }
        protected ServiceBase(IServiceProvider services, IUnitOfWork uow) : base(services, uow)
        {
        }

        public TRepository Repository
        {
            get
            {
                if (this._repository == null)
                    this._repository = (TRepository)this.Services.GetService(typeof(TRepository));

                return this._repository;
            }
        }
    }

}
