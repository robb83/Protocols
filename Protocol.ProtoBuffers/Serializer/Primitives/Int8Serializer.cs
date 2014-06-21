
namespace Protocol.ProtoBuffers.Serializer
{
    public class Int8Serializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            return (byte)reader.ReadVarint();
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            writer.Write((uint)(sbyte)instance);
        }
    }
}
