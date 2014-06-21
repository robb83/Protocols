using System;

namespace Protocol.ProtoBuffers.Serializer
{
    public class PrimitivesDecorator : IProtoSerializer
    {
        private IProtoSerializer itemSerializer;

        public PrimitivesDecorator(IProtoSerializer itemSerializer)
        {
            if (itemSerializer == null)
            {
                throw new ArgumentNullException("itemSerializer");
            }

            this.itemSerializer = itemSerializer;
        }

        public object Read(ProtoReader reader)
        {
            int fn = reader.ReadTag();

            if (fn == 0)
            {
                throw new InvalidProgramException();
            }

            return this.itemSerializer.Read(reader);
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            writer.SetFieldNumber(1);
            this.itemSerializer.Writer(writer, instance);
        }
    }
}
