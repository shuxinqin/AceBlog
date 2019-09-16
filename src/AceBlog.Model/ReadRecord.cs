using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Entity
{
    public class ReadRecord
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int ReaderId { get; set; }
        public DateTime ReadTime { get; set; }
    }
}
