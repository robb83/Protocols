using System;

namespace Protocol.ProtoBuffers.Serializer
{
    public class DateTimeSerializer : IProtoSerializer
    {
        //TODO: TimeZone

        public object Read(ProtoReader reader)
        {
            long ticks = 0;

            int offset = 0;
            byte[] buffer = reader.ReadByteArray();

            ticks |= (long)buffer[offset++];
            ticks |= (long)buffer[offset++] << 8;
            ticks |= (long)buffer[offset++] << 16;
            ticks |= (long)buffer[offset++] << 24;
            ticks |= (long)buffer[offset++] << 32;
            ticks |= (long)buffer[offset++] << 40;
            ticks |= (long)buffer[offset++] << 48;
            ticks |= (long)buffer[offset++] << 56;

            DateTimeKind kind = (DateTimeKind)buffer[offset++];

            if (kind == DateTimeKind.Local)
            {
                return new DateTime(ticks, DateTimeKind.Utc).ToLocalTime();
            }

            return new DateTime(ticks, kind);
        }

        public void Writer(ProtoWriter writer, object instance)
        {
            DateTime dt = (DateTime)instance;
            byte kind = (byte)dt.Kind;
            long ticks;

            if (dt.Kind == DateTimeKind.Local)
            {
                ticks = dt.ToUniversalTime().Ticks;
            }
            else
            {
                ticks = dt.Ticks;
            }
            
            int offset = 0;
            byte[] buffer = new byte[9];

            buffer[offset++] = (byte)((ticks) & 0xFF);
            buffer[offset++] = (byte)((ticks >> 8) & 0xFF);
            buffer[offset++] = (byte)((ticks >> 16) & 0xFF);
            buffer[offset++] = (byte)((ticks >> 24) & 0xFF);
            buffer[offset++] = (byte)((ticks >> 32) & 0xFF);
            buffer[offset++] = (byte)((ticks >> 40) & 0xFF);
            buffer[offset++] = (byte)((ticks >> 48) & 0xFF);
            buffer[offset++] = (byte)((ticks >> 56) & 0xFF);
            buffer[offset++] = kind;

            writer.Write(buffer);
        }
    }
}
