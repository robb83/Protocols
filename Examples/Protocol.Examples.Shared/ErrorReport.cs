using Protocol.ProtoBuffers;
using System;

namespace Protocol.Examples.Shared
{
    [ProtoContract]
    public class ErrorReport
    {
        [ProtoMember(1)]
        public int Code { get; set; }
        [ProtoMember(2)]
        public String Message { get; set; }
    }
}
