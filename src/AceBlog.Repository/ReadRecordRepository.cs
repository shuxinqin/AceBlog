using Ace;
using Ace.Domain;
using Chloe;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Repository
{
    class ReadRecordRepository : RepositoryBase<ReadRecord>, IReadRecordRepository
    {
        public ReadRecordRepository(IDbContext dbContext) : base(dbContext)
        {

        }

        public bool HasRead(int userId, int blogId)
        {
            return this.Query().Where(a => a.ReaderId == userId && a.BlogId == blogId).Any();
        }
    }
}
