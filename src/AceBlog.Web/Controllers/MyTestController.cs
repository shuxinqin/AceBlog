using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using AceBlog.Web.Common;

namespace AceBlog.Web.Controllers
{
    public class MyTestController : WebController
    {
        public MyTestController()
        {

        }

        [Route("MyTest1/u/{userId}")]
        public ActionResult Index(string userId)
        {
            return this.Content($"123-{userId}");
            return View();
        }

        public ActionResult GetIP()
        {
            var forwardedIP = this.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            string remoteIP = this.HttpContext.Connection.RemoteIpAddress.ToString();


            string ip = this.HttpContext.GetClientIP();
            return this.SuccessData(new { forwardedIP, remoteIP, ip });
        }

        [ResponseCache(Duration = WebConsts.PageOutputCacheDuration, VaryByQueryKeys = new string[] { "s" })]
        public ActionResult Test(string s)
        {
            this.Logger.LogInformation(s);
            return this.SuccessData(s);
        }
    }
}