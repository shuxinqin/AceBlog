using Ace;
using Ace.Domain;
using Chloe;
using AceBlog.Entity;
using AceBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Repository
{
    class BlogRepository : RepositoryBase<Blog>, IBlogRepository
    {
        public BlogRepository(IDbContext dbContext) : base(dbContext)
        {

        }

        public PagedData<BlogModel> GetPublishedBlogs(Pagination page, int? authorId)
        {
            var result = this.Query().InnerJoin<User>((blog, user) => blog.AuthorId == user.Id).Select((blog, user) => new { Blog = blog, Author = user }).WhereIfNotNull(authorId, a => a.Blog.AuthorId == authorId).Where(a => a.Blog.Status == BlogStatus.Published).OrderByDesc(a => a.Blog.Id).Select(a =>
                   new BlogModel()
                   {
                       Id = a.Blog.Id,
                       AuthorId = a.Blog.AuthorId,
                       Title = a.Blog.Title,
                       Summary = a.Blog.Summary,
                       Tag = a.Blog.Tag,
                       ReadCount = a.Blog.ReadCount,
                       Status = a.Blog.Status,
                       CreateTime = a.Blog.CreateTime,
                       PublishTime = a.Blog.PublishTime,
                       Author = new AuthorModel()
                       {
                           Id = a.Author.Id,
                           NickName = a.Author.NickName,
                           HeadPhoto = a.Author.HeadPhoto,
                           Description = a.Author.Description,
                           RegisterTime = a.Author.RegisterTime,
                       }
                   }).TakePageData(page);

            return result;
        }
        public PagedData<BlogModel> GetUserBlogs(Pagination page, int authorId)
        {
            var result = this.Query().Where(a => a.AuthorId == authorId).Where(a => a.Status != BlogStatus.Deleted).OrderByDesc(a => a.Id).TakePageData(page);

            var ret = new PagedData<BlogModel>(result.TotalCount, result.CurrentPage, result.PageSize);
            ret.Models.AddRange(result.Models.Select(a => BlogModel.Create(a)));
            return ret;
        }

        public Blog GetBlogDetail(int id, int? authorId)
        {
            var data = this.Query().InnerJoin<BlogContent>((blog, blogContent) => blog.Id == blogContent.Id).Select((blog, blogContent) => new { Blog = blog, BlogContent = blogContent }).Where(a => a.Blog.Id == id && a.Blog.Status != BlogStatus.Deleted).WhereIfNotNull(authorId, a => a.Blog.AuthorId == authorId.Value).OrderBy(a => a.Blog.Id).Select(a => new { Blog = a.Blog, BlogContent = a.BlogContent }).FirstOrDefault();

            if (data == null)
                return null;

            User author = this.DbContext.Query<User>().Where(a => a.Id == data.Blog.AuthorId).First();

            Blog ret = data.Blog;
            ret.Content = data.BlogContent;
            ret.Author = author;

            this.DbContext.TrackEntity(ret);
            this.DbContext.TrackEntity(ret.Content);

            return ret;
        }

        public BlogNeighbour GetNeighbours(int id, int authorId)
        {
            BlogNeighbour ret = new BlogNeighbour();

            var q = this.Query().Where(a => a.AuthorId == authorId && a.Status == BlogStatus.Published);
            ret.Prev = BlogModel.Create(q.Where(a => a.Id < id).OrderByDesc(a => a.Id).FirstOrDefault());
            ret.Next = BlogModel.Create(q.Where(a => a.Id > id).OrderBy(a => a.Id).FirstOrDefault());

            return ret;
        }

        public void IncreaseReaderCount(int blogId)
        {
            this.DbContext.Update<Blog>(a => a.Id == blogId, a => new Blog() { ReadCount = a.ReadCount + 1 });
        }
    }
}
