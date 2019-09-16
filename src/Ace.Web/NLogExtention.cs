using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ace.Web.NLog
{
    public static class NLogExtention
    {
        public static void ConfigureNLog(IHostingEnvironment env, ILoggerFactory loggerFactory, string configFileRelativePath = "nlog.config")
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog(configFileRelativePath);
        }
    }
}
