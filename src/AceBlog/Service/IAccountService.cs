using Ace.Application;
using AceBlog.Model;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Service
{
    public interface IAccountService : IService
    {
        UserModel Register(RegisterInput input);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password">经过md5加密后的密码</param>
        /// <param name="user"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CheckLogin(string loginName, string password, out UserModel user, out string msg);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword">明文</param>
        /// <param name="newPassword">明文</param>
        void ChangePassword(int userId, string oldPassword, string newPassword);
        void ModifyInfo(ModifyAccountInfoInput input);
    }

}
