using Protocol.ProtoBuffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Examples.Shared
{
    [ProtoContract]
    public class Response
    {
        public static Response SuccessResponse = new Response { Success = true };

        public static Response GenericError = new Response { Success = false, Error = new ErrorReport { Code = -1, Message = "Something is not right" } };

        [ProtoMember(1)]
        public bool Success { get; set; }

        [ProtoMember(2)]
        public ErrorReport Error { get; set; }
    }
}
