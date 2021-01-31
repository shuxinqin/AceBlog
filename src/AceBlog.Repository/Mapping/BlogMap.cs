using Chloe.Entity;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Repository.Mapping
{
    class BlogMap : EntityTypeBuilder<Blog>
    {
        public BlogMap()
        {
            this.Property(a => a.CreateTime).UpdateIgnore();
        }
    }
}
