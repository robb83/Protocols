using System;

namespace Protocol.ProtoBuffers.Serializer
{
    public class TimeSpanSerializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            return new TimeSpan((long)reader.ReadVarint64());
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            TimeSpan ts = (TimeSpan)instance;            
            writer.Write((ulong)ts.Ticks);
        }
    }
}
