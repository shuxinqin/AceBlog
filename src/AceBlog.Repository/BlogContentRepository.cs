using Ace.Domain;
using Chloe;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Repository
{
    class BlogContentRepository : RepositoryBase<BlogContent>, IBlogContentRepository
    {
        public BlogContentRepository(IDbContext dbContext) : base(dbContext)
        {

        }
    }
}
