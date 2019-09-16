using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Model
{
    public class BlogNeighbour
    {
        public BlogModel Prev { get; set; }
        public BlogModel Next { get; set; }
    }
}
