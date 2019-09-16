using Ace.Domain;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Repository
{
    public interface IReadRecordRepository : IRepository<ReadRecord>
    {
        bool HasRead(int userId, int blogId);
    }
}
