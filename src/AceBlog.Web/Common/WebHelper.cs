using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AceBlog.Web.Common
{
    public static class WebHelper
    {
        public static UserSession GetCurrentUser(this HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return null;

            UserSession session = UserSession.Parse(httpContext.User);
            return session;
        }
    }
}
