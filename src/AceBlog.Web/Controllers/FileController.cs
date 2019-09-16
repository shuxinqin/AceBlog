using Ace;
using Ace.Caching;
using Ace.Web.Mvc.Authorization;
using AceBlog.Model;
using AceBlog.Entity;
using AceBlog.Service;
using AceBlog.Service.Events;
using AceBlog.Web.Common;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AceBlog.Web.Controllers
{
    [LoginAttribute]
    public class FileController : WebController
    {
        IHostingEnvironment _env;
        public FileController(IHostingEnvironment env)
        {
            this._env = env;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Upload()
        {
            return this.SaveFile("public");
        }

        IActionResult SaveFile(string catalogName)
        {
            //Thread.Sleep(3000);
            IFormFile file = this.Request.Form.Files.FirstOrDefault();

            if (file == null)
            {
                return this.FailedMsg("请选择上传文件");
            }

            string fileName = file.FileName;
            string fileExt = FileExtName(fileName);
            if (!IsLegalFile(fileExt))
            {
                return this.FailedMsg($"禁止上传 {fileName} 文件"); ;
            }

            //数据大小
            long fileLength = file.Length;
            if (fileLength > FileAllowedLength)
            {
                return this.FailedMsg("超出30M大文件不允许上传！"); ;
            }

            DateTime dt = DateTime.Now;

            string newFileName = Guid.NewGuid().ToString("N") + fileExt;

            string webRootPath = this._env.WebRootPath;

            //文件相对路径
            string relativeDir = $"/upload/{catalogName}/{dt.ToString("yyyyMM")}/";
            string saveDir = ConbinePath(webRootPath, relativeDir);
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            string savePath = Path.Combine(saveDir, newFileName);
            using (var stream = new FileStream(savePath, FileMode.CreateNew))
            {
                file.CopyTo(stream);
            }

            string relativePath = ConbinePath(relativeDir, newFileName);
            return this.SuccessData(relativePath);
        }


        static string ConbinePath(string part1, string part2)
        {
            if (part1[part1.Length - 1] != '/' && part2[0] != '/')
                return part1 + "/" + part2;

            if (part1[part1.Length - 1] == '/' && part2[0] == '/')
                return part1.Substring(0, part1.Length - 1) + part2;

            return part1 + part2;
        }

        public static int FileAllowedLength { get { return 30 * 1024 * 1024; } }

        static bool IsLegalFile(string ext)
        {
            if (string.IsNullOrEmpty(ext))
                return false;

            ext = ext.ToLower();
            return AllowFiles().Contains(ext);
        }
        static List<string> AllowFiles()
        {
            //文件允许格式
            string[] filetype = { ".rar", ".doc", ".docx", ".zip", ".7z", ".ppt", ".pptx", ".pdf", ".xls", ".xlsx", ".txt", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg", ".ogg", ".mov", ".wmv", ".mp4", ".webm", ".chm", ".jpg", ".jpeg", ".png", ".gif", ".mm" };

            List<string> fileExts = new List<string>(filetype);
            return fileExts;
        }
        string FileExtName(string fileName)
        {
            //在不同浏览器下，filename有时候指的是文件名，有时候指的是全路径，所有这里要要统一。
            if (fileName.LastIndexOf("\\", StringComparison.Ordinal) > -1)
            {
                //IndexOf 有时候会受到特殊字符的影响而判断错误。加上这个就纠正了。
                fileName = fileName.Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            }
            return Path.GetExtension(fileName.ToLower());
        }
    }
}