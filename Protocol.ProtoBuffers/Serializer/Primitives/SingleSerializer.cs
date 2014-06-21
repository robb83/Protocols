
namespace Protocol.ProtoBuffers.Serializer
{
    public class SingleSerializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            uint raw = reader.ReadVarint();
            unsafe
            {
                return *((float*)&raw);
            }
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            unsafe
            {
                float n = (float)instance;
                writer.Write(*((uint*)&n));
            }
        }
    }
}
