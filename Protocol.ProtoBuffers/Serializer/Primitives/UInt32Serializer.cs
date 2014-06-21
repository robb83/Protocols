
namespace Protocol.ProtoBuffers.Serializer
{
    public class UInt32Serializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            return reader.ReadVarint();
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            writer.Write((uint)instance);
        }
    }
}
