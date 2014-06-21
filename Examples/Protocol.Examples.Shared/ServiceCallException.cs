using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Examples.Shared
{
    public class ServiceCallException : Exception
    {
        private int errorCode;

        public ServiceCallException(int errorCode, String message)
            :base(message)
        {

        }

        public ServiceCallException(int errorCode, String message, Exception innerException)
            : base(message, innerException)
        {

        }

        public int ErrorCode { get { return this.errorCode; } }
    }
}
