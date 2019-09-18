using Ace;
using Ace.Application;
using AceBlog.Model;
using AceBlog.Entity;
using AceBlog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Service
{
    class BlogService : ServiceBase<IBlogRepository>, IBlogService
    {
        IUserRepository _userRepository;
        IBlogContentRepository _blogContentRepository;
        public BlogService(IServiceProvider services, IUserRepository userRepository, IBlogContentRepository blogContentRepository) : base(services)
        {
            this._userRepository = userRepository;
            this._blogContentRepository = blogContentRepository;
        }

        public PagedData<BlogModel> GetPublishedBlogs(Pagination page, int? authorId)
        {
            return this.Repository.GetPublishedBlogs(page, authorId);
        }
        public PagedData<BlogModel> GetUserBlogs(Pagination page, int authorId)
        {
            return this.Repository.GetUserBlogs(page, authorId);
        }
        public BlogModel GetBlogDetail(int id)
        {
            return BlogModel.Create(this.Repository.GetBlogDetail(id));
        }

        public BlogNeighbour GetNeighbours(int id, int authorId)
        {
            return this.Repository.GetNeighbours(id, authorId);
        }

        public BlogModel AddAndPublish(AddBlogInput input)
        {
            User user = this._userRepository.Get(input.AuthorId);

            Blog blog = user.CreateBlog(input);
            user.PublishBlog(blog);

            this.AddBlog(blog);

            BlogModel ret = BlogModel.Create(blog);
            return ret;
        }

        public BlogModel Publish(PublishBlogInput input)
        {
            Blog blog = this.Repository.GetBlogDetail(input.Id, input.AuthorId);

            blog.Author.UpdateBlog(blog, input);

            if (blog.Status == BlogStatus.Unpublished)
            {
                blog.Author.PublishBlog(blog);
            }

            this.UpdateBlog(blog);

            BlogModel ret = BlogModel.Create(blog);
            return ret;
        }

        public void PublishBlog(int id, int authorId)
        {
            Blog blog = this.Repository.GetBlogDetail(id, authorId);

            blog.Author.PublishBlog(blog);
            this.Repository.Update(blog);
        }
        public void UnpublishBlog(int id, int authorId)
        {
            Blog blog = this.Repository.GetBlogDetail(id, authorId);

            blog.Author.UnpublishBlog(blog);
            this.Repository.Update(blog);
        }

        public BlogModel SaveDraft(SaveBlogDraftInput input)
        {
            Blog blog = null;
            if (input.Id == null)
            {
                User user = this._userRepository.Get(input.AuthorId);
                blog = user.CreateBlog(input);
                this.AddBlog(blog);
            }
            else
            {
                blog = this.Repository.GetBlogDetail(input.Id.Value);
                if (blog.AuthorId != input.AuthorId || blog.Status != BlogStatus.Unpublished)
                    throw new ServiceException("无效操作");

                blog.Author.UpdateBlog(blog, input);
                this.UpdateBlog(blog);
            }

            BlogModel ret = BlogModel.Create(blog);
            return ret;
        }

        public void DeleteUserBlog(int id, int authorId)
        {
            Blog blog = this.Repository.GetBlogDetail(id, authorId);

            blog.Author.DeleteBlog(blog);
            this.Repository.Update(blog);
        }

        void UpdateBlog(Blog blog)
        {
            this.UseTransaction(() =>
            {
                this.Repository.Update(blog);
                this._blogContentRepository.Update(blog.Content);
            });
        }
        void AddBlog(Blog blog)
        {
            this.UseTransaction(() =>
            {
                this.Repository.Insert(blog);

                blog.Content.Id = blog.Id;
                this._blogContentRepository.Insert(blog.Content);
            });
        }
    }
}
