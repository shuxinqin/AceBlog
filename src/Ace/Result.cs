using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    public class Result
    {
        public Result()
        {
        }
        public Result(bool success)
        {
            this.Success = success;
        }

        public Result(bool success, string msg)
        {
            this.Success = success;
            this.Msg = msg;
        }

        /// <summary>
        /// 默认为 true
        /// </summary>
        public bool Success { get; set; } = true;
        public string Msg { get; set; }

        public static Result Create(string msg = null)
        {
            Result result = Create(true, msg);
            return result;
        }
        public static Result Create(bool success, string msg = null)
        {
            Result result = new Result(success);
            result.Msg = msg;
            return result;
        }

        public static Result<T> Create<T>(T data)
        {
            Result<T> result = Create<T>(true, data);
            return result;
        }
        public static Result<T> Create<T>(bool success)
        {
            Result<T> result = Create<T>(success, default(T));
            return result;
        }
        public static Result<T> Create<T>(bool success, T data)
        {
            Result<T> result = new Result<T>(success);
            result.Data = data;
            return result;
        }

        public static Result FailMsg(string msg = null)
        {
            return Create(false, msg);
        }
        public static Result SuccessMsg(string msg = null)
        {
            return Create(true, msg);
        }
    }
}
