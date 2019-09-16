using Ace;
using Ace.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AceBlog.Web.Infrastructure
{
    /// <summary>
    /// 全局异常处理中间件
    /// </summary>
    public class GlobalErrorHandlingMiddleware
    {
        RequestDelegate _next;
        ILogger<GlobalErrorHandlingMiddleware> _logger;
        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        Task FailedMsg(HttpContext context, string msg = null)
        {
            Result retResult = Result.FailMsg(msg);
            string json = JsonConvert.SerializeObject(retResult);
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(json);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await this._next(httpContext);
            }
            catch (Exception ex)
            {
                if (httpContext.Request.IsAjaxRequest())
                {
                    string msg = null;

                    if (ex is InvalidInputException || ex is ServiceException || ex is DomainException)
                    {
                        msg = ex.Message;
                    }
                    else
                    {
                        this.LogException(httpContext, ex);
                        msg = "服务器错误";

#if DEBUG
                        throw;
#endif
                    }

                    await this.FailedMsg(httpContext, msg);
                    return;
                }
                else
                {
                    //对于非 ajax 请求
                    this.LogException(httpContext, ex);

#if DEBUG
                    throw;
#endif

                    httpContext.Response.ContentType = "text/html;charset=utf-8";
                    httpContext.Response.StatusCode = 500;
                    await httpContext.Response.WriteAsync("服务器错误 500");

                    return;
                }
            }
        }

        /// <summary>
        /// 将错误记录进日志
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        void LogException(HttpContext context, Exception ex)
        {
            ILogger logger = this._logger;
            string errorMsg = this.BuildErrorMsg(context, ex);
            logger.LogError(errorMsg);
        }
        string BuildErrorMsg(HttpContext context, Exception ex)
        {
            var request = context.Request;
            string visitUrl = request.Path;
            string method = request.Method.ToUpper();
            string urlParamters = string.Empty;

            if (method == "GET")
            {
                urlParamters = request.QueryString.Value;
            }

            string errorMsg = ex.Message + "\n" + ex.StackTrace;
            string log = $"{visitUrl}#{method}#{urlParamters}#{errorMsg}";

            return log;
        }
    }
}
