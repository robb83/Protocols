
namespace Protocol.ProtoBuffers.Serializer
{
    public class BooleanSerializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            uint value = reader.ReadVarint();
            return value == 1;
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            bool value = (bool)instance;

            if (value)
            {
                writer.Write((uint)1);
            }
            else
            {
                writer.Write((uint)0);
            }
        }
    }
}
