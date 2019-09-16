using Ace;
using Ace.Caching;
using Ace.Web.Mvc.Authorization;
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
    [LoginAttribute]
    public class BlogController : WebController<IBlogService>
    {
        IUserService _userService;
        IMediator _mediator;

        public BlogController(IUserService userService, IMediator mediator)
        {
            this._userService = userService;
            this._mediator = mediator;
        }

        /// <summary>
        /// 阅览博客
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowNotLoginAttribute]
        [Route("{userId}/Blog/{id}")]
        public async Task<ActionResult> Blog(int userId, int id)
        {
            BlogModel blog = this.Service.GetBlogDetail(id);
            if (blog == null || blog.AuthorId != userId)
                return this.NotFoundView();

            bool isAuthor = this.CurrentSession != null && this.CurrentSession.UserId == blog.AuthorId;
            if (blog.Status == BlogStatus.Unpublished && !isAuthor)
                return this.NotFoundView();

            //增加阅读数
            if (this.CurrentSession != null)
                await _mediator.Publish(new ReadBlogEvent() { BlogId = blog.Id, ReaderId = this.CurrentSession.UserId.Value, ReadTime = DateTime.Now });

            BlogNeighbour blogNeighbour = this.Service.GetNeighbours(blog.Id, blog.AuthorId);

            this.ViewBag.Blog = blog;
            this.ViewBag.BlogNeighbour = blogNeighbour;
            return this.View();
        }

        [Route("Blog/Edit")]
        [Route("Blog/Edit/{id}")]
        public ActionResult EditBlog(int? id)
        {
            this.ViewBag.EditBlogId = id;

            return this.View();
        }

        /// <summary>
        /// 新增并发布博客
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAndPublish(AddBlogInput input)
        {
            input.AuthorId = this.CurrentSession.UserId.Value;

            var blog = this.Service.AddAndPublish(input);
            return this.SuccessData(blog);
        }

        /// <summary>
        /// 更新或发布未发布的博客
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Publish(PublishBlogInput input)
        {
            input.AuthorId = this.CurrentSession.UserId.Value;

            var blog = this.Service.Publish(input);
            return this.SuccessData(blog);
        }
        /// <summary>
        /// 下线指定id博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Unpublish(int id)
        {
            this.Service.UnpublishBlog(id, this.CurrentSession.UserId.Value);
            return this.UpdateSuccessMsg();
        }

        /// <summary>
        /// 发布指定id博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Pub(int id)
        {
            this.Service.PublishBlog(id, this.CurrentSession.UserId.Value);
            return this.DeleteSuccessMsg();
        }

        /// <summary>
        /// 保存为草稿
        /// </summary>
        /// <returns></returns>
        [Route("Blog/SaveDraft")]
        [HttpPost]
        public ActionResult SaveDraft(SaveBlogDraftInput input)
        {
            input.AuthorId = this.CurrentSession.UserId.Value;
            var blog = this.Service.SaveDraft(input);
            return this.SuccessData(blog);
        }

        [Route("Blog/Mine/{id}")]
        public ActionResult GetMyBlog(int id)
        {
            BlogModel blog = this.Service.GetBlogDetail(id);

            if (blog != null && blog.AuthorId != this.CurrentSession.UserId)
                blog = null;

            return this.SuccessData(blog);
        }

        public ActionResult List()
        {
            return this.View();
        }

        public ActionResult GetUserBlogs(int page = 1, int pageSize = 20)
        {
            PagedData<BlogModel> result = this.Service.GetUserBlogs(new Pagination(page, pageSize), this.CurrentSession.UserId.Value);

            return this.SuccessData(result);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            this.Service.DeleteUserBlog(id, this.CurrentSession.UserId.Value);
            return this.DeleteSuccessMsg();
        }
    }
}