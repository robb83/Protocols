using System;
using System.Collections;

namespace Protocol.ProtoBuffers.Serializer
{
    public class ListSerializer : IProtoSerializer
    {
        private Type listType;
        private Type elementType;
        private bool isArray;
        private TypeDescription typeDescription;

        public ListSerializer(bool isArray, Type elementType, Type listType, TypeDescription description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("itemSerializer");
            }

            if (isArray == false && listType == null)
            {
                throw new ArgumentNullException("listType");
            }

            if (isArray == true && elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }

            this.listType = listType;
            this.elementType = elementType;
            this.isArray = isArray;
            this.typeDescription = description;
        }

        public object Read(ProtoReader reader)
        {
            IList list = (isArray ? new ArrayList() : (IList)Activator.CreateInstance(listType));

            int fieldNumber;
            int messageSize = reader.BeginSubMessage();
            while ((fieldNumber = reader.ReadTag()) > 0)
            {
                object value;

                if (reader.WireType == WireType.Null)
                {
                    value = reader.ReadNull();
                }
                else 
                {
                    value = this.typeDescription.NestedMessageSerializer.Read(reader);
                }

                list.Add(value);
            }
            reader.EndSubMessage(messageSize);

            if (isArray)
            {
                int length = list.Count;

                Array array = Array.CreateInstance(elementType, length);
                list.CopyTo(array, 0);

                return array;
            }

            return list;
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            IEnumerable enumerable = (IEnumerable)instance;

            int messageId = writer.BeginSubMessage();

            foreach (var e in enumerable)
            {
                if (e == null)
                {
                    writer.SetFieldNumber(1);
                    writer.WriteNull();
                }
                else
                {
                    writer.SetFieldNumber(1);
                    this.typeDescription.NestedMessageSerializer.Writer(writer, e);
                }
            }

            writer.EndSubMessage(messageId);
        }
    }
}
