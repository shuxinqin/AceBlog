using Ace;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AceBlog.Model
{
    public class BlogInputBase : ValidationModel
    {
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(50, ErrorMessage = "标题太短或太长")]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Tag { get; set; }
        [Required(ErrorMessage = "内容不能为空")]
        public string Html { get; set; }
        public string MarkdownCode { get; set; }

        public int AuthorId { get; set; }
    }

    public class AddBlogInput : BlogInputBase
    {

    }

    public class PublishBlogInput : BlogInputBase
    {
        public int Id { get; set; }
    }

    public class SaveBlogDraftInput : BlogInputBase
    {
        public int? Id { get; set; }
    }
}
