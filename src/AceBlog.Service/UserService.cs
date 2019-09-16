using Ace.Application;
using AceBlog.Model;
using AceBlog.Entity;
using AceBlog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Service
{
    class UserService : ServiceBase<IUserRepository>, IUserService
    {
        public UserService(IServiceProvider services) : base(services)
        {

        }

        public UserModel Get(int userId)
        {
            return UserModel.Create(this.Repository.Get(userId));
        }
    }
}
