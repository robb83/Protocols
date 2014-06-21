using System;

namespace Protocol.ProtoBuffers.Serializer
{
    public class StringSerializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            if (reader.WireType == WireType.Null)
            {
                return reader.ReadNull();
            }
            else
            {
                return reader.ReadString();
            }
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            if (instance == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.Write((String)instance);
            }
        }
    }
}
