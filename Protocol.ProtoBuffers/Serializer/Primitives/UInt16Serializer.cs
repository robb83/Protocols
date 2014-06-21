
namespace Protocol.ProtoBuffers.Serializer
{
    public class UInt16Serializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            return (short)reader.ReadVarint();
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            writer.Write((uint)(ushort)instance);
        }
    }
}
