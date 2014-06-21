
namespace Protocol.ProtoBuffers.Serializer
{
    public class Int32Serializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            return (int)reader.ReadVarint();
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            writer.Write((uint)(int)instance);
        }
    }
}
