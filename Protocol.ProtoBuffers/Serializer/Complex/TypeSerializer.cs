using System;
using System.Collections.Generic;
using System.Reflection;

namespace Protocol.ProtoBuffers.Serializer
{
    public class TypeSerializer : IProtoSerializer
    {
        private Type objectType;
        private LinkedList<IMemberSerializer> serializer;

        public TypeSerializer(Type objectType, LinkedList<IMemberSerializer> serializer)
        {
            if (objectType == null)
                throw new ArgumentNullException("objectType");
            if (serializer == null)
                throw new ArgumentNullException("serializer");
            if (objectType.IsAbstract)
                throw new InvalidProgramException();

            foreach(var s1 in serializer)
            {
                int count = 0;
                foreach (var s2 in serializer)
                {
                    if (s2.FieldNumber == s1.FieldNumber)
                        ++count;
                }

                if (count > 1)
                {
                    throw new InvalidProgramException();
                }
            }

            this.objectType = objectType;
            this.serializer = serializer;
        }

        IMemberSerializer GetSerializerByFieldNumber(int fieldNumber)
        {
            foreach(var i in serializer)
            {
                if (i.FieldNumber == fieldNumber)
                    return i;
            }

            return null;
        }

        public object Read(ProtoReader reader)
        {
            object instance = Activator.CreateInstance(objectType);
            int fn;

            while((fn = reader.ReadTag()) > 0)
            {
                IMemberSerializer serializer = GetSerializerByFieldNumber(fn);

                if (serializer == null)
                    continue;

                serializer.Read(reader, instance);
            }

            return instance;
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            foreach(var m in this.serializer)
            {
                m.Write(writer, instance);
            }
        }

        public class FieldSerializer : IMemberSerializer
        {
            int fieldNumber;
            FieldInfo fieldInfo;
            TypeDescription typeDescription;

            public FieldSerializer(int fieldNumber, FieldInfo fieldInfo, TypeDescription description)
            {
                this.fieldNumber = fieldNumber;
                this.fieldInfo = fieldInfo;
                this.typeDescription = description;
            }

            public void Read(ProtoReader reader, object instance)
            {
                object value;

                if (reader.WireType == WireType.Null)
                {
                    value = reader.ReadNull();
                }
                else
                {
                    value = typeDescription.NestedMessageSerializer.Read(reader);
                }

                fieldInfo.SetValue(instance, value);
            }

            public void Write(ProtoWriter writer, object instance)
            {
                object value = fieldInfo.GetValue(instance);

                if (value == null)
                {
                    writer.SetFieldNumber(this.fieldNumber);
                    writer.WriteNull();
                }
                else
                {
                    writer.SetFieldNumber(this.fieldNumber);
                    typeDescription.NestedMessageSerializer.Writer(writer, value);
                }
            }

            public int FieldNumber
            {
                get { return this.fieldNumber; }
            }
        }

        public class PropertySerializer : IMemberSerializer
        {
            int fieldNumber;
            PropertyInfo propertyInfo;
            TypeDescription typeDescription;

            public PropertySerializer(int fieldNumber, PropertyInfo propertyInfo, TypeDescription description)
            {
                this.fieldNumber = fieldNumber;
                this.propertyInfo = propertyInfo;
                this.typeDescription = description;
            }

            public void Read(ProtoReader reader, object instance)
            {
                object value;

                if (reader.WireType == WireType.Null)
                {
                    value = reader.ReadNull();
                }
                else
                {
                    value = typeDescription.NestedMessageSerializer.Read(reader);
                }

                propertyInfo.SetValue(instance, value, null);
            }

            public void Write(ProtoWriter writer, object instance)
            {
                object value = propertyInfo.GetValue(instance, null);

                if (value == null)
                {
                    writer.SetFieldNumber(this.fieldNumber);
                    writer.WriteNull();
                }
                else 
                {
                    writer.SetFieldNumber(this.fieldNumber);
                    typeDescription.NestedMessageSerializer.Writer(writer, value);
                }
            }
            public int FieldNumber
            {
                get { return this.fieldNumber; }
            }
        }

        public interface IMemberSerializer
        {
            int FieldNumber { get; }
            void Read(ProtoReader reader, object instance);
            void Write(ProtoWriter writer, object instance);
        }
    }
}
