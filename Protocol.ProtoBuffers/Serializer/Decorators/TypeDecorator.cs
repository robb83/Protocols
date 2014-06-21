using System;

namespace Protocol.ProtoBuffers.Serializer
{
    public class TypeDecorator : IProtoSerializer
    {
        private IProtoSerializer itemSerializer;

        public TypeDecorator(IProtoSerializer itemSerializer)
        {
            if (itemSerializer == null)
            {
                throw new ArgumentNullException("itemSerializer");
            }

            this.itemSerializer = itemSerializer;
        }

        public object Read(ProtoReader reader)
        {
            if (reader.WireType == WireType.Null)
            {
                return reader.ReadNull();
            }
            
            object value = null;

            int messageSize = reader.BeginSubMessage();
            value = this.itemSerializer.Read(reader);
            reader.EndSubMessage(messageSize);

            return value;
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            if (instance == null)
            {
                writer.WriteNull();
            }
            else
            {
                int messageId = writer.BeginSubMessage();
                this.itemSerializer.Writer(writer, instance);
                writer.EndSubMessage(messageId);
            }
        }
    }
}
