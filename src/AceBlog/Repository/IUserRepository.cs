using Ace.Domain;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AceBlog.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        UserDetail GetByLoginName(string loginName);
        UserDetail GetUserDetail(int id, bool tracking = false);
        void AddUser(UserDetail user);
        void UpdateUser(UserDetail user);
    }
}
