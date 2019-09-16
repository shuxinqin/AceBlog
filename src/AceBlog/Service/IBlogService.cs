using Ace;
using Ace.Application;
using AceBlog.Model;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Service
{
    public interface IBlogService : IService
    {
        PagedData<BlogModel> GetPublishedBlogs(Pagination page, int? authorId);
        PagedData<BlogModel> GetUserBlogs(Pagination page, int authorId);
        BlogModel GetBlogDetail(int id);
        /// <summary>
        /// 获取上一篇和下一篇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        BlogNeighbour GetNeighbours(int id, int authorId);

        /// <summary>
        /// 新增并发布博客
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        BlogModel AddAndPublish(AddBlogInput input);

        /// <summary>
        /// 更新或发布未发布的博客
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        BlogModel Publish(PublishBlogInput input);
        /// <summary>
        /// 下线博客
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorId"></param>
        void UnpublishBlog(int id, int authorId);
        void PublishBlog(int id, int authorId);

        /// <summary>
        /// 存为草稿
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        BlogModel SaveDraft(SaveBlogDraftInput input);

        /// <summary>
        /// 删除博客
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorId"></param>
        void DeleteUserBlog(int id, int authorId);
    }
}
