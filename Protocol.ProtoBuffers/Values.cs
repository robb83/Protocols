using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.ProtoBuffers
{
    public static class Values
    {
        public const int SizePosition = 2;
        public const int HeaderSize = 6; // revision + size
        public const short ProtocolRevision = 0x01;
    }
}
