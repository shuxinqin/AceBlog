using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Model
{
    public class BlogModel
    {
        public AuthorModel Author { get; set; }

        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Tag { get; set; }
        public int ReadCount { get; set; }
        public BlogStatus Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? PublishTime { get; set; }
        public string Html { get; set; }
        public string MarkdownCode { get; set; }

        public string GetBlogUrl()
        {
            return $"/{this.AuthorId}/Blog/{this.Id}";
        }

        public static BlogModel Create(Blog blog)
        {
            if (blog == null)
                return null;

            BlogModel ret = new BlogModel();
            ret.Id = blog.Id;
            ret.AuthorId = blog.AuthorId;
            ret.Title = blog.Title;
            ret.Summary = blog.Summary;
            ret.Tag = blog.Tag;
            ret.ReadCount = blog.ReadCount;
            ret.Status = blog.Status;
            ret.CreateTime = blog.CreateTime;
            ret.PublishTime = blog.PublishTime;
            ret.Html = blog.Content?.Html;
            ret.MarkdownCode = blog.Content?.MarkdownCode;

            ret.Author = AuthorModel.Create(blog.Author);

            return ret;
        }
    }
}
