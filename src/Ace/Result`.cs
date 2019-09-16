using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    public class Result<T> : Result
    {
        public Result()
        {
        }
        public Result(bool success)
            : base(success)
        {
        }
        public Result(bool success, T data)
            : base(success)
        {
            this.Data = data;
        }
        public T Data { get; set; }
    }
}
