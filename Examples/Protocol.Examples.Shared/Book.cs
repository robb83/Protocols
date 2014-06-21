using Protocol.ProtoBuffers;
using System;

namespace Protocol.Examples.Shared
{
    [ProtoContract]
    public class Book
    {
        [ProtoMember(1)]
        public String ISBN { get; set; }
        [ProtoMember(2)]
        public String Title { get; set; }
        [ProtoMember(3)]
        public String Author { get; set; }
        [ProtoMember(4)]
        public int PublishYear { get; set; }
        [ProtoMember(5)]
        public String Publisher { get; set; }
    }
}
