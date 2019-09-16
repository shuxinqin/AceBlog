using Ace.Domain;
using AceBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Entity
{
    public enum BlogStatus
    {
        Unpublished = 0,
        Published = 1,
        Deleted = 2
    }

    public class Blog
    {
        public BlogContent Content { get; set; }
        public User Author { get; set; }

        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Tag { get; set; }
        public BlogStatus Status { get; set; }
        public int ReadCount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? PublishTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DeleteTime { get; set; }

        public void SetPublished()
        {
            if (this.Status != BlogStatus.Unpublished)
            {
                throw new DomainException();
            }

            this.Status = BlogStatus.Published;
            this.PublishTime = DateTime.Now;
        }
        public void SetUnpublished()
        {
            if (this.Status != BlogStatus.Published)
            {
                throw new DomainException();
            }

            this.Status = BlogStatus.Unpublished;
        }
        public void SetDeleted()
        {
            if (this.Status == BlogStatus.Deleted)
            {
                throw new DomainException();
            }

            this.Status = BlogStatus.Deleted;
            this.DeleteTime = DateTime.Now;
        }
        public void Update(BlogInputBase input)
        {
            input.Validate();

            this.Title = input.Title;
            this.Summary = input.Summary;
            this.Tag = input.Tag;

            this.Content.Html = input.Html;
            this.Content.MarkdownCode = input.MarkdownCode;

            this.UpdateTime = DateTime.Now;
        }
    }
}
