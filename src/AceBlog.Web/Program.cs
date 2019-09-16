using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AceBlog.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string contentRoot = Directory.GetCurrentDirectory();

            var hostBuilder = WebHost.CreateDefaultBuilder(args);
            var host = hostBuilder
                .UseContentRoot(contentRoot)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
