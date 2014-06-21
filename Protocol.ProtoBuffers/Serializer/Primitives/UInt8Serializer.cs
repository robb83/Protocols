
namespace Protocol.ProtoBuffers.Serializer
{
    public class UInt8Serializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            return (byte)reader.ReadVarint();
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            writer.Write((uint)(byte)instance);
        }
    }
}
