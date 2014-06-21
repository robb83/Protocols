
namespace Protocol.ProtoBuffers.Serializer
{
    public class ByteArraySerializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            return reader.ReadByteArray();
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            writer.Write((byte[])instance);
        }
    }
}
