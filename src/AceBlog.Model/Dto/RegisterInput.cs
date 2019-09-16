using Ace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Model
{
    public class RegisterInput
    {
        public string AccountName { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public string VerifyCode { get; set; }
    }
}
