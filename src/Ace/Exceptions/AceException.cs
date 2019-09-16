using System;
using System.Collections.Generic;
using System.Text;

namespace Ace
{
    public class AceException : Exception
    {
        public AceException()
        {
        }
        public AceException(string message)
            : base(message)
        {
        }
    }
}
