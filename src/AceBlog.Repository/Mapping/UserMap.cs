﻿using Chloe.Entity;
using AceBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceBlog.Repository.Mapping
{
    class UserMap : EntityTypeBuilder<User>
    {
        public UserMap()
        {
            this.MapTo("Users");

            this.Property(a => a.RegisterTime).UpdateIgnore();
        }
    }
    class UserDetailMap : EntityTypeBuilder<UserDetail>
    {
        public UserDetailMap()
        {
            this.MapTo("Users");
        }
    }
}
