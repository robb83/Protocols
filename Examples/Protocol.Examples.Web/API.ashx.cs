using Protocol.Examples.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Protocol.Examples.Web
{
    /// <summary>
    /// Summary description for ApplicationInterface
    /// </summary>
    public class WebApplicationInterface : Protocol.Examples.Web.BaseApplicationInterface
    {
        public WebApplicationInterface()
            :base(typeof(Protocol.Examples.Web.BookStoreApi))
        {

        }
    }
}