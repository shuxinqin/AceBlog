using Ace;
using Ace.Domain;
using Chloe;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Repository
{
    class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext)
        {

        }

        public UserDetail GetByLoginName(string loginName)
        {
            loginName = loginName.ToLower();

            var q = this.DbContext.Query<UserDetail>();
            if (AceUtils.IsMobilePhone(loginName))
            {
                q = q.Where(a => a.MobilePhone == loginName);
            }
            else if (AceUtils.IsEmail(loginName))
            {
                q = q.Where(a => a.Email == loginName);
            }
            else
            {
                q = q.Where(a => a.AccountName == loginName);
            }

            return q.FirstOrDefault();
        }

        public UserDetail GetUserDetail(int id, bool tracking = false)
        {
            return this.DbContext.QueryByKey<UserDetail>(id, tracking);
        }

        public void AddUser(UserDetail user)
        {
            this.DbContext.Insert(user);
        }
        public void UpdateUser(UserDetail user)
        {
            this.DbContext.Update(user);
        }
    }
}
