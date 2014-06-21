using System;
using System.Text;

namespace Protocol.ProtoBuffers.Serializer
{
    public class UriSerializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            return new Uri(reader.ReadString());
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            writer.Write(((Uri)instance).ToString());
        }
    }
}
