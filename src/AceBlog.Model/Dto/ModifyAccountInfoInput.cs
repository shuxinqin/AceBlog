using Ace;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AceBlog.Model
{
    public class ModifyAccountInfoInput : ValidationModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "昵称不能为空")]
        [StringLength(18, MinimumLength = 2, ErrorMessage = "昵称太短或太长")]
        public string NickName { get; set; }
        public string Name { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Description { get; set; }
    }
}
