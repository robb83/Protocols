
namespace Protocol.ProtoBuffers.Serializer
{
    public class NullableSerializer : IProtoSerializer
    {
        private TypeDescription typeDescription;

        public NullableSerializer(TypeDescription description)
        {
            this.typeDescription = description;
        }

        public object Read(ProtoReader reader)
        {
            if (reader.WireType == WireType.Null)
            {
                return reader.ReadNull();
            }

            return typeDescription.NestedMessageSerializer.Read(reader);
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            if (instance == null)
            {
                writer.WriteNull();
            }
            else
            {
                typeDescription.NestedMessageSerializer.Writer(writer, instance);
            }
        }
    }
}
