using Ace;
using Ace.Caching;
using AceBlog.Entity;
using AceBlog.Model;
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
    public class HomeController : WebController
    {
        IBlogService _blogService;
        ICache _cache;

        public HomeController(IBlogService blogService, IUserService userService, ICache cache)
        {
            this._blogService = blogService;
            this._cache = cache;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1)
        {
            Pagination pagination = new Pagination(page, 20);

            PagedData<BlogModel> blogs = null;
            if (pagination.Page == 1)
            {
                //对第一页数据做缓存
                blogs = this._cache.GetOrDefault(WebConsts.HOME_FIRST_PAGE_BLOGS) as PagedData<BlogModel>;

                if (blogs == null)
                {
                    blogs = this._blogService.GetPublishedBlogs(pagination, null);

                    int cacheSeconds = 20;
#if DEBUG
                    cacheSeconds = 1;
#endif

                    this._cache.Set(WebConsts.HOME_FIRST_PAGE_BLOGS, blogs, null, TimeSpan.FromSeconds(cacheSeconds));
                }
            }
            else
            {
                blogs = this._blogService.GetPublishedBlogs(pagination, null);
            }

            this.ViewBag.Blogs = blogs;
            return View();
        }

        public ActionResult Error()
        {
            return this.View();
        }
    }
}