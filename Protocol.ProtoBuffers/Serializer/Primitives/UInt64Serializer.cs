
namespace Protocol.ProtoBuffers.Serializer
{
    public class UInt64Serializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            return (ulong)reader.ReadVarint64();
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            writer.Write((ulong)instance);
        }
    }
}
