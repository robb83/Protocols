
namespace Protocol.ProtoBuffers.Serializer
{
    public class DoubleSerializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            ulong raw = reader.ReadVarint64();
            unsafe
            {
                return *((double*)&raw);
            }
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            unsafe
            {
                double n = (double)instance;
                writer.Write(*((ulong*)&n));
            }
        }
    }
}
