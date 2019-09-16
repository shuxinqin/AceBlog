using Ace.Web.Mvc.Authorization;
using AceBlog.Model;
using AceBlog.Entity;
using AceBlog.Service;
using AceBlog.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Ace.VerifyCode;
using System.Drawing;
using Ace.Caching;
using Microsoft.AspNetCore.Http;

namespace AceBlog.Web.Controllers
{
    public class AccountController : WebController<IAccountService>
    {
        IUserService _userService;
        public AccountController(IUserService userService)
        {
            this._userService = userService;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string loginName, string password/*经过md5加密后的密码*/ )
        {
            if (loginName.IsNullOrEmpty() || password.IsNullOrEmpty())
                return this.FailedMsg("用户名/密码不能为空");

            loginName = loginName.Trim();

            UserModel user;
            string msg;
            if (!this.Service.CheckLogin(loginName, password, out user, out msg))
            {
                return this.FailedMsg(msg);
            }

            UserSession session = new UserSession();
            session.UserId = user.Id;
            session.AccountName = user.AccountName;
            session.Name = user.NickName;

            this.CurrentSession = session;

            return this.SuccessMsg(msg);
        }
        public ActionResult Logout([FromServices]IMemoryCache memoryCache)
        {
            this.CurrentSession = null;
            return this.Redirect("/");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VerifyCode()
        {
            string code;
            Bitmap bitmap = new ValidateCoder().CreateImage(4, out code);
            byte[] bytes = bitmap.ToBytes();

            string cacheKey = this.GenVerifyCodeCacheKey();
            this.HttpContext.Session.SetString(cacheKey, code);

            return this.File(bytes, @"image/Gif");
        }
        string GenVerifyCodeCacheKey()
        {
            string cacheKey = $"__VERIFYCODE_{this.HttpContext.Session.Id}";
            return cacheKey;
        }

        [HttpPost]
        public ActionResult Register(RegisterInput input)
        {
            if (input.VerifyCode.IsNullOrEmpty())
                return this.FailedMsg("请输入验证码");

            string cacheKey = this.GenVerifyCodeCacheKey();
            string cacheCode = this.HttpContext.Session.GetString(cacheKey);
            this.HttpContext.Session.Remove(cacheKey);

            if (cacheCode.IsNullOrEmpty() || cacheCode.ToLower() != input.VerifyCode.ToLower())
            {
                return this.FailedMsg("验证码错误，请重新输入");
            }

            UserModel user = this.Service.Register(input);

            UserSession session = new UserSession();
            session.UserId = user.Id;
            session.AccountName = user.AccountName;
            session.Name = user.NickName;

            this.CurrentSession = session;

            return this.SuccessData(user);
        }

        [LoginAttribute]
        public ActionResult Index()
        {
            UserModel user = this._userService.Get(this.CurrentSession.UserId.Value);
            this.ViewBag.User = user;

            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldPassword">明文</param>
        /// <param name="newPassword">明文</param>
        /// <returns></returns>
        [LoginAttribute]
        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            this.CreateService<IAccountService>().ChangePassword(this.CurrentSession.UserId.Value, oldPassword, newPassword);
            return this.SuccessMsg("密码修改成功");
        }

        [LoginAttribute]
        [HttpPost]
        public ActionResult ModifyInfo(ModifyAccountInfoInput input)
        {
            input.Id = this.CurrentSession.UserId.Value;
            this.Service.ModifyInfo(input);
            return this.SuccessMsg("修改成功");
        }
    }
}