using System;
using System.IO;
using System.Text;

namespace Protocol.ProtoBuffers
{
    public class ProtoReader : IDisposable
    {
        private Stream source;
        private int depth;
        private int position;
        private int readLimit;
        private byte[] buffer;
        private int messageSize;
        private int fieldNumber;
        private WireType wireType;
        private int subMessageLimit;
        private int protocolRevision;

        public ProtoReader(Stream source)
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            this.depth = 0;
            this.position = 0;
            this.source = source;
            this.buffer = BufferPool.LockBuffer();
            this.wireType = WireType.None;
            this.fieldNumber = 0;
            this.subMessageLimit = 0;
        }
        
        public int ReadMessageHeader()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            // mindig az üzenet méretével kezdődik
            int readed = source.Read(this.buffer, 0, Values.HeaderSize);

            if (readed == Values.HeaderSize)
            {
                uint value = 0;
                int revision = 0;

                revision |= this.buffer[this.position++] << 8;
                revision |= this.buffer[this.position++];

                value |= (uint)this.buffer[this.position++];
                value |= (uint)this.buffer[this.position++] << 8;
                value |= (uint)this.buffer[this.position++] << 16;
                value |= (uint)this.buffer[this.position++] << 24;

                this.messageSize = (int)value;

                if (this.buffer.Length < this.messageSize)
                {
                    // ha nincs elég nagy buffer, akkor kérünk nagyobbat
                    this.buffer = BufferPool.Change(this.buffer, 0, this.messageSize);
                }

                this.protocolRevision = revision;
                this.readLimit = source.Read(this.buffer, 0, this.messageSize);
                this.position = 0;
            }
            else
            {
                this.messageSize = 0;
            }

            return this.messageSize;
        }

        public int ReadTag()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.wireType != ProtoBuffers.WireType.None && this.fieldNumber > 0)
            {
                SkipField();
            }

            ulong tag = PrivateReadVarint();

            this.fieldNumber = (int)(tag >> 3);
            this.wireType = (WireType)(tag & 0x07);

            return this.fieldNumber;
        }

        public object ReadNull()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            this.wireType = ProtoBuffers.WireType.None;
            return null;
        }

        private void SkipField()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            int length;

            switch(this.wireType)
            {
                case ProtoBuffers.WireType.Fixed32:
                    ReadInt32();
                    break;
                case ProtoBuffers.WireType.Fixed64:
                    ReadInt64();
                    break;
                case ProtoBuffers.WireType.VarInt:
                    ReadVarint64();
                    break;
                case ProtoBuffers.WireType.Bytes:
                    length = (int)PrivateReadVarint();
                    this.position += length;
                    break;
                case ProtoBuffers.WireType.Message:
                    length = (int)PrivateReadFixed32();
                    this.position += length;
                    break;
                case ProtoBuffers.WireType.Null:
                case ProtoBuffers.WireType.None:
                default:
                    break;
            }
        }

        public byte[] ReadByteArray()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.wireType != WireType.Bytes)
                throw new InvalidOperationException();

            int length = (int)PrivateReadVarint();
            byte[] data = new byte[length];

            Array.Copy(this.buffer, this.position, data, 0, length);
            this.position += length;

            this.wireType = WireType.None;

            return data;
        }

        public String ReadString()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.wireType != ProtoBuffers.WireType.Bytes)
                throw new InvalidProgramException();

            byte[] bytes = ReadByteArray();
            String value = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            this.wireType = ProtoBuffers.WireType.None;

            return value;
        }

        public uint ReadVarint()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.wireType != WireType.VarInt)
                throw new InvalidOperationException();

            ulong value = PrivateReadVarint();
            this.wireType = ProtoBuffers.WireType.None;
            return (uint)value;
        }

        public ulong ReadVarint64()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.wireType != WireType.VarInt)
                throw new InvalidOperationException();

            ulong value = PrivateReadVarint();
            this.wireType = ProtoBuffers.WireType.None;
            return value;
        }
        
        public uint ReadInt32()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.wireType != WireType.Fixed32)
                throw new InvalidOperationException();

            ulong value = PrivateReadFixed32();
            this.wireType = ProtoBuffers.WireType.None;
            return (uint)value;
        }

        public ulong ReadInt64()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.wireType != WireType.Fixed64)
                throw new InvalidOperationException();

            ulong value = PrivateReadFixed64();
            this.wireType = ProtoBuffers.WireType.None;
            return (uint)value;
        }

        public int BeginSubMessage()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.wireType != WireType.Message)
                throw new InvalidOperationException();

            int currentMessageLimit = this.subMessageLimit;
            this.subMessageLimit = this.position + (int)PrivateReadFixed32();
            ++this.depth;
            this.wireType = ProtoBuffers.WireType.None;

            return currentMessageLimit;
        }

        public void EndSubMessage(int subMessageId)
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            --this.depth;

            if (this.depth < 0)
                throw new InvalidProgramException();
            
            this.subMessageLimit = subMessageId;
        }

        public WireType WireType
        {
            get { return this.wireType; }
        }
        
        public void Dispose()
        {
            if (depth != 0)
            {
                throw new InvalidProgramException();
            }
        }
        
        private uint PrivateReadFixed32()
        {
            int limit = (this.depth > 0 ? this.subMessageLimit : this.readLimit);

            if (limit - this.position < 4)
            {
                throw new InvalidProgramException();
            }

            uint value = 0;

            value |= (uint)this.buffer[this.position++];
            value |= (uint)this.buffer[this.position++] << 8;
            value |= (uint)this.buffer[this.position++] << 16;
            value |= (uint)this.buffer[this.position++] << 24;
            
            return value;
        }

        private ulong PrivateReadFixed64()
        {
            int limit = (this.depth > 0 ? this.subMessageLimit : this.readLimit);

            if (limit - this.position < 8)
            {
                throw new InvalidProgramException();
            }

            ulong value = 0;

            value |= (uint)this.buffer[this.position++];
            value |= (uint)this.buffer[this.position++] << 8;
            value |= (uint)this.buffer[this.position++] << 16;
            value |= (uint)this.buffer[this.position++] << 24;
            value |= (uint)this.buffer[this.position++] << 32;
            value |= (uint)this.buffer[this.position++] << 40;
            value |= (uint)this.buffer[this.position++] << 48;
            value |= (uint)this.buffer[this.position++] << 56;

            return value;
        }

        private ulong PrivateReadVarint()
        {
            const ulong sevenbitmask = 0x7F;

            int limit = (this.depth > 0 ? this.subMessageLimit : this.readLimit);

            ulong value = 0;

            for (int shift = 0; position < limit; shift += 7)
            {
                byte current = buffer[position++];

                value |= (current & sevenbitmask) << shift;

                if ((current & 0x80) == 0)
                    break;
            }

            return value;
        }
    }
}
