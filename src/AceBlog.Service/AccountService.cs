using Ace;
using Ace.Application;
using Ace.Security;
using AceBlog.Model;
using AceBlog.Entity;
using AceBlog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Service
{
    class AccountService : ServiceBase, IAccountService
    {
        IUserRepository _userRepository;
        public AccountService(IServiceProvider services, IUserRepository userRepository) : base(services)
        {
            this._userRepository = userRepository;
        }

        public void ChangePassword(int userId, string oldPassword, string newPassword)
        {
            PasswordHelper.EnsurePasswordLegal(newPassword);

            UserDetail user = this._userRepository.GetUserDetail(userId, true);
            user.ChangePassword(oldPassword, newPassword);
            this._userRepository.UpdateUser(user);
        }

        public bool CheckLogin(string loginName, string password, out UserModel user, out string msg)
        {
            user = null;
            msg = null;

            loginName.NotNullOrEmpty();
            password.NotNullOrEmpty();

            UserDetail account = this._userRepository.GetByLoginName(loginName);

            if (account == null)
            {
                msg = "账户不存在，请重新输入";
                return false;
            }

            string dbPassword = PasswordHelper.EncryptMD5Password(password, account.SecretKey);
            if (dbPassword != account.Password)
            {
                msg = "密码不正确，请重新输入";
                return false;
            }

            user = UserModel.Create(account);
            return true;
        }

        public UserModel Register(RegisterInput input)
        {
            AceUtils.EnsureAccountNameLegal(input.AccountName);

            PasswordHelper.EnsurePasswordLegal(input.Password);

            if (input.NickName.IsNullOrEmpty() || input.NickName.Length < 2 || input.NickName.Length > 15)
                throw new InvalidInputException("昵称太短或太长");

            string accountName = input.AccountName.ToLower();
            AceUtils.EnsureAccountNameLegal(accountName);
            bool exists = this._userRepository.Query().Where(a => a.AccountName == accountName).Any();
            if (exists)
                throw new InvalidInputException("用户名[{0}]已存在".ToFormat(input.AccountName));

            UserDetail user = new UserDetail();
            user.AccountName = accountName;
            user.NickName = input.NickName;

            string userSecretkey = UserHelper.GenUserSecretkey();
            string encryptedPassword = PasswordHelper.Encrypt(input.Password, userSecretkey);

            user.SecretKey = userSecretkey;
            user.Password = encryptedPassword;

            user.HeadPhoto = "/content/images/avatar.png";
            user.RegisterTime = DateTime.Now;

            this._userRepository.AddUser(user);

            return UserModel.Create(user);
        }

        public void ModifyInfo(ModifyAccountInfoInput input)
        {
            User user = this._userRepository.Get(input.Id, true);
            user.ModifyInfo(input);
            this._userRepository.Update(user);
        }
    }
}
