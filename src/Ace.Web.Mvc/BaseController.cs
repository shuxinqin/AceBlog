using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Web.Mvc
{
    public abstract class BaseController : Controller
    {
        ILogger _logger = null;
        public ILogger Logger
        {
            get
            {
                if (this._logger == null)
                {
                    ILoggerFactory loggerFactory = this.HttpContext.RequestServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
                    ILogger logger = loggerFactory.CreateLogger(this.GetType().FullName);
                    this._logger = logger;
                }

                return this._logger;
            }
        }

        protected virtual T CreateService<T>()
        {
            return (T)this.HttpContext.RequestServices.GetService(typeof(T));
        }

        [NonAction]
        public ContentResult JsonContent(object obj)
        {
            string json = JsonHelper.Serialize(obj);
            return base.Content(json);
        }

        [NonAction]
        public ContentResult SuccessData(object data = null)
        {
            Result<object> result = Result.Create<object>(true, data);
            return this.JsonContent(result);
        }
        [NonAction]
        public ContentResult SuccessMsg(string msg = "操作成功")
        {
            Result result = new Result(true, msg);
            return this.JsonContent(result);
        }
        [NonAction]
        public ContentResult AddSuccessData(object data, string msg = "添加成功")
        {
            Result<object> result = Result.Create<object>(true, data);
            result.Msg = msg;
            return this.JsonContent(result);
        }
        [NonAction]
        public ContentResult AddSuccessMsg(string msg = "添加成功")
        {
            return this.SuccessMsg(msg);
        }
        [NonAction]
        public ContentResult UpdateSuccessMsg(string msg = "更新成功")
        {
            return this.SuccessMsg(msg);
        }
        [NonAction]
        public ContentResult DeleteSuccessMsg(string msg = "删除成功")
        {
            return this.SuccessMsg(msg);
        }
        [NonAction]
        public ContentResult FailedMsg(string msg = null)
        {
            Result retResult = new Result(false, msg);
            return this.JsonContent(retResult);
        }
    }
}
