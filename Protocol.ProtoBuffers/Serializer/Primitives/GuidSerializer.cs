using System;

namespace Protocol.ProtoBuffers.Serializer
{
    public class GuidSerializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            return new Guid(reader.ReadByteArray());
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            var bytes = ((Guid)instance).ToByteArray();
            writer.Write(bytes);
        }
    }
}
