
namespace Protocol.ProtoBuffers.Serializer
{
    public class Int64Serializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            return (long)reader.ReadVarint64();
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            writer.Write((ulong)(long)instance);
        }
    }
}
