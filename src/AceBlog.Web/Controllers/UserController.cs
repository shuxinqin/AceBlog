using Ace;
using Ace.Caching;
using AceBlog.Model;
using AceBlog.Entity;
using AceBlog.Service;
using AceBlog.Service.Events;
using AceBlog.Web.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AceBlog.Web.Controllers
{
    public class UserController : WebController<IUserService>
    {
        IBlogService _blogService;

        public UserController(IBlogService blogService)
        {
            this._blogService = blogService;
        }

        /// <summary>
        /// 用户主页
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("{userId}")]
        public ActionResult Index(int userId, int page = 1)
        {
            Pagination pagination = new Pagination(page, 20);
            PagedData<BlogModel> blogs = this._blogService.GetPublishedBlogs(pagination, userId);
            this.ViewBag.Blogs = blogs;

            UserModel owner = this.Service.Get(userId);
            this.ViewBag.Owner = owner;

            return View();
        }
    }
}