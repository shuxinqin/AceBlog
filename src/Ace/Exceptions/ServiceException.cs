using System;
using System.Collections.Generic;
using System.Text;

namespace Ace
{
    public class ServiceException : Exception
    {
        public ServiceException()
        {
        }
        public ServiceException(string message)
            : base(message)
        {
        }
    }
}
