using System;

namespace Protocol.ProtoBuffers.Serializer
{
    public class EnumSerializer : IProtoSerializer
    {
        private Type enumType;
        private TypeCode typeCode;

        public EnumSerializer(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }

            this.enumType = enumType;
            this.typeCode = Type.GetTypeCode(enumType);

            switch(this.typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.SByte:
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public object Read(ProtoReader reader)
        {
            switch (this.typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                    return Enum.ToObject(enumType, reader.ReadVarint());
                case TypeCode.Int64:
                case TypeCode.UInt64:
                default:                    
                    return Enum.ToObject(enumType, reader.ReadVarint64());
            }
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            switch (this.typeCode)
            {
                case TypeCode.Byte:
                    writer.Write((uint)(byte)instance);
                    break;
                case TypeCode.SByte:
                    writer.Write((uint)(sbyte)instance);
                    break;
                case TypeCode.Int16:
                    writer.Write((uint)(short)instance);
                    break;
                case TypeCode.Int32:
                    writer.Write((uint)(int)instance);
                    break;
                case TypeCode.UInt16:
                    writer.Write((uint)(ushort)instance);
                    break;
                case TypeCode.UInt32:
                    writer.Write((uint)instance);
                    break;
                case TypeCode.Int64:
                    writer.Write((ulong)(long)instance);
                    break;
                case TypeCode.UInt64:
                    writer.Write((ulong)instance);
                    break;
            }            
        }
    }
}
