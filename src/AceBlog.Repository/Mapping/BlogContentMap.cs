using Chloe.Entity;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Repository.Mapping
{
    class BlogContentMap : EntityTypeBuilder<BlogContent>
    {
        public BlogContentMap()
        {
            this.Property(a => a.Id).IsAutoIncrement(false);
        }
    }
}
