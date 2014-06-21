using System;

namespace Protocol.ProtoBuffers.Serializer
{
    public class DecimalSerializer : IProtoSerializer
    {
        public object Read(ProtoReader reader)
        {
            int[] bits = new int[4];
            byte[] buffer = reader.ReadByteArray();

            Buffer.BlockCopy(buffer, 0, bits, 0, buffer.Length);

            return new Decimal(bits);
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            int[] bits = Decimal.GetBits((decimal)instance);
            byte[] buffer = new byte[Buffer.ByteLength(bits)];
            Buffer.BlockCopy(bits, 0, buffer, 0, buffer.Length);

            writer.Write(buffer);
        }
    }
}
