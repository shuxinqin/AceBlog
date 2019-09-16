using Ace.Application;
using AceBlog.Model;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Service
{
    public interface IUserService : IService
    {
        UserModel Get(int userId);
    }
}
