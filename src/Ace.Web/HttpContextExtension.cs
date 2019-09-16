using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Claims;
using System.IO;
using System.Web;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtension
    {
        static Dictionary<string, string> _contentTypes = new Dictionary<string, string>();

        static HttpContextExtension()
        {
            //http://tool.oschina.net/commons
            _contentTypes.Add(".gif", "image/gif");
            _contentTypes.Add(".jpe", "image/jpeg");
            _contentTypes.Add(".jpeg", "image/jpeg");
            _contentTypes.Add(".jpg", "image/jpeg");
            _contentTypes.Add(".ico", "image/x-icon");
            _contentTypes.Add(".img", "application/x-img");
            _contentTypes.Add(".png", "image/png");

            _contentTypes.Add(".doc", "application/msword");
            _contentTypes.Add(".docx", "application/msword");
            _contentTypes.Add(".ppt", "application/x-ppt");
            _contentTypes.Add(".pdf", "application/pdf");
            _contentTypes.Add(".xls", "application/x-xls");
            _contentTypes.Add(".xlsx", "application/vnd.ms-excel");

            _contentTypes.Add(".txt", "text/plain");
            _contentTypes.Add(".html", "text/html");
            _contentTypes.Add(".svg", "text/xml");
        }

        /// <summary>
        /// 将一个带 ~ 开头的路径转成相应的相对路径，如 ~/msg/index --> /msg/index 或者 ~/msg/index --> /virtual_path/msg/index
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="contentPath"></param>
        /// <returns></returns>
        public static string Content(this HttpContext httpContext, string contentPath)
        {
            if (string.IsNullOrEmpty(contentPath))
            {
                return null;
            }
            else if (contentPath[0] == '~')
            {
                var segment = new PathString(contentPath.Substring(1));
                var applicationPath = httpContext.Request.PathBase;

                return applicationPath.Add(segment).Value;
            }

            return contentPath;
        }

        public static bool IsAjaxRequest(this HttpContext httpContext)
        {
            return httpContext.Request.IsAjaxRequest();
        }

        public static string GetClientIP(this HttpContext httpContext)
        {
            var ip = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = httpContext.Connection.RemoteIpAddress.ToString();
            }

            if (ip == "::1")
                ip = "127.0.0.1";

            return ip;
        }

        /// <summary>
        /// 获取请求的完整url(不包含#部分)
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetRequestUrl(this HttpContext httpContext)
        {
            HttpRequest httpRequest = httpContext.Request;

            string scheme = httpRequest.Scheme;
            string host = httpRequest.Host.Value;
            string pathBase = httpRequest.PathBase.Value;
            string path = httpRequest.Path.Value;
            string queryString = httpRequest.QueryString.Value;

            string url = $"{scheme}://{host}{pathBase}{path}{queryString}";

            return url;
        }
        /// <summary>
        /// 获取请求 url 中的 scheme 和 host 部分，如 http://localhost:50465/msg/index --> http://localhost:50465
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetSchemeHost(this HttpContext httpContext)
        {
            HttpRequest httpRequest = httpContext.Request;

            string scheme = httpRequest.Scheme;
            string host = httpRequest.Host.Value;

            string url = $"{scheme}://{host}";

            return url;
        }
        public static string GetUrlReferer(this HttpContext httpContext)
        {
            string referrer = httpContext.Request.Headers["Referer"];
            return referrer;
        }

        /// <summary>
        /// 根据文件扩展名获取相应的 content-type
        /// </summary>
        /// <param name="extension">带 . 的</param>
        /// <returns></returns>
        public static string GetContentTypeByFileExtension(string extension)
        {
            string contentType;
            if (string.IsNullOrEmpty(extension) || !_contentTypes.TryGetValue(extension.ToLower(), out contentType))
                return "application/octet-stream";

            return contentType;
        }
        /// <summary>
        /// 根据文件名获取相应的 content-type
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileContentType(string fileName)
        {
            string extension = "";
            if (!string.IsNullOrEmpty(fileName))
                extension = Path.GetExtension(fileName);

            return GetContentTypeByFileExtension(extension);
        }

        /// <summary>
        /// 写入文件流
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="stream"></param>
        /// <param name="outputFileName"></param>
        public static void WriteFile(this HttpContext httpContext, MemoryStream stream, string outputFileName)
        {
            outputFileName = HttpUtility.UrlEncode(outputFileName);
            string ext = Path.GetExtension(outputFileName);
            string contentType = HttpContextExtension.GetContentTypeByFileExtension(ext);

            httpContext.Response.Clear();

            httpContext.Response.Headers.Add("Content-Disposition", "attachment;filename=" + outputFileName);
            httpContext.Response.Headers["Content-Encoding"] = "utf-8";
            httpContext.Response.ContentType = contentType;

            byte[] bytes = stream.ToArray();
            httpContext.Response.Body.Write(bytes, 0, bytes.Length);
            httpContext.Response.Body.Flush();
        }
    }
}
