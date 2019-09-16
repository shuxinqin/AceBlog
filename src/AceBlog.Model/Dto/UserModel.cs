using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Model
{
    public class UserModel
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

        public static UserModel Create(User user)
        {
            if (user == null)
                return null;

            UserModel ret = new UserModel();
            ret.Id = user.Id;
            ret.AccountName = user.AccountName;
            ret.MobilePhone = user.MobilePhone;
            ret.Email = user.Email;
            ret.NickName = user.NickName;
            ret.Name = user.Name;
            ret.Gender = user.Gender;
            ret.Birthday = user.Birthday;
            ret.HeadPhoto = user.HeadPhoto;
            ret.Description = user.Description;
            ret.RegisterTime = user.RegisterTime;

            return ret;
        }
    }
}
