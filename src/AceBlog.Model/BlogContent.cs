using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Entity
{
    public class BlogContent
    {
        /// <summary>
        /// Blog.Id
        /// </summary>
        public int Id { get; set; }
        public string Html { get; set; }
        public string MarkdownCode { get; set; }
    }
}
