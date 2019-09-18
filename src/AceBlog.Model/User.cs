using Ace;
using Ace.Domain;
using Ace.Security;
using AceBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Entity
{
    public enum Gender
    {
        Male = 1,
        Femal = 2
    }

    public class UserDetail : User
    {
        public string Password { get; set; }
        public string SecretKey { get; set; }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            string encryptedOldPassword = PasswordHelper.Encrypt(oldPassword, this.SecretKey);

            if (encryptedOldPassword != this.Password)
                throw new InvalidInputException("旧密码不正确");

            string newUserSecretkey = UserHelper.GenUserSecretkey();
            string newEncryptedPassword = PasswordHelper.Encrypt(newPassword, newUserSecretkey);

            this.Password = newEncryptedPassword;
            this.SecretKey = newUserSecretkey;
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string HeadPhoto { get; set; }
        public string Description { get; set; }
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 创建新博客，但未发布
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Blog CreateBlog(BlogInputBase input)
        {
            input.Validate();

            Blog blog = new Blog();
            BlogContent blogContent = new BlogContent();
            blog.Content = blogContent;

            blog.Title = input.Title;
            blog.Summary = input.Summary;
            blog.Tag = input.Tag;
            blog.AuthorId = this.Id;

            blog.CreateTime = DateTime.Now;
            blog.Status = BlogStatus.Unpublished;

            blogContent.Html = input.Html;
            blogContent.MarkdownCode = input.MarkdownCode;

            blog.Author = this;

            return blog;
        }

        /// <summary>
        /// 更新博客基本信息和内容
        /// </summary>
        /// <param name="blog"></param>
        /// <param name="input"></param>
        public void UpdateBlog(Blog blog, BlogInputBase input)
        {
            blog.Update(input);
        }

        /// <summary>
        /// 发布博客
        /// </summary>
        /// <param name="blog"></param>
        public void PublishBlog(Blog blog)
        {
            blog.SetPublished();
        }
        public void UnpublishBlog(Blog blog)
        {
            blog.SetUnpublished();
        }

        public void DeleteBlog(Blog blog)
        {
            blog.SetDeleted();
        }

        public void ModifyInfo(ModifyAccountInfoInput input)
        {
            input.Validate();
            this.NickName = input.NickName;
            this.Name = input.Name;
            this.Gender = input.Gender;
            this.Birthday = input.Birthday;
            this.Description = input.Description;
        }
    }
}
