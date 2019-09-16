using Ace;
using Ace.Domain;
using AceBlog.Entity;
using AceBlog.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AceBlog.Repository
{
    public interface IBlogRepository : IRepository<Blog>
    {
        PagedData<BlogModel> GetPublishedBlogs(Pagination page, int? authorId);
        PagedData<BlogModel> GetUserBlogs(Pagination page, int authorId);
        Blog GetBlogDetail(int id, int? authorId = null);
        BlogNeighbour GetNeighbours(int id, int authorId);
        void IncreaseReaderCount(int blogId);
    }
}
