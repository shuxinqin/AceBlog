using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Model
{
    public class AuthorModel
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string HeadPhoto { get; set; }
        public string Description { get; set; }
        public DateTime RegisterTime { get; set; }

        public string GetHomeUrl()
        {
            return $"/{this.Id}";
        }

        public static AuthorModel Create(User user)
        {
            if (user == null)
                return null;

            AuthorModel ret = new AuthorModel();
            ret.Id = user.Id;
            ret.NickName = user.NickName;
            ret.HeadPhoto = user.HeadPhoto;
            ret.Description = user.Description;
            ret.RegisterTime = user.RegisterTime;

            return ret;
        }
    }
}
