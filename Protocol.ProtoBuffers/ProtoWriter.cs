using System;
using System.IO;
using System.Text;

namespace Protocol.ProtoBuffers
{
    public class ProtoWriter : IDisposable
    {
        Stream sourceStream;
        byte[] buffer;
        int position;
        int depth;
        int fieldNumber;

        public ProtoWriter(Stream source)
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (source == null || !source.CanWrite)
                throw new ArgumentException("source");

            this.depth = 0;
            this.position = Values.HeaderSize; // space for header
            this.sourceStream = source;
            this.buffer = BufferPool.LockBuffer();
            this.buffer[0] = (Values.ProtocolRevision >> 8) & 0xFF;
            this.buffer[1] = (Values.ProtocolRevision & 0xFF);
        }

        public int GetFieldNumber() { return this.fieldNumber; }

        public void SetFieldNumber(int fieldNumber)
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.fieldNumber != 0)
                throw new InvalidProgramException();

            this.fieldNumber = fieldNumber;
        }

        public void WriteNull()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.fieldNumber == 0)
                throw new InvalidProgramException();

            PrivateWriteVarint((uint)((this.fieldNumber << 3) | (int)WireType.Null));
            this.fieldNumber = 0;
        }

        public void Write(uint value)
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.fieldNumber == 0)
                throw new InvalidProgramException();

            PrivateWriteVarint((uint)((this.fieldNumber << 3) | (int)WireType.VarInt));
            PrivateWriteVarint(value);
            
            this.fieldNumber = 0;
        }

        public void Write(ulong value)
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.fieldNumber == 0)
                throw new InvalidProgramException();

            PrivateWriteVarint((uint)((this.fieldNumber << 3) | (int)WireType.VarInt));
            PrivateWriteVarint(value);

            this.fieldNumber = 0;
        }

        public void Write(byte[] value)
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.fieldNumber == 0)
                throw new InvalidProgramException();

            PrivateWriteVarint((uint)((this.fieldNumber << 3) | (int)WireType.Bytes));
            PrivateWriteArray(value);

            this.fieldNumber = 0;
        }

        public void Write(String value)
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.fieldNumber == 0)
                throw new InvalidProgramException();

            PrivateWriteVarint((uint)((this.fieldNumber << 3) | (int)WireType.Bytes));
            PrivateWriteArray(Encoding.UTF8.GetBytes(value));

            this.fieldNumber = 0;
        }
        
        public int BeginSubMessage()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (this.fieldNumber == 0)
                throw new InvalidProgramException();
            
            if (this.depth > 15)
            {
                throw new InvalidProgramException();
            }

            ++depth;

            PrivateWriteVarint((uint)((this.fieldNumber << 3) | (int)WireType.Message));
            this.fieldNumber = 0;

            Ensure(4); // for space

            int startPosition = position;            
            position += 4; // reserv space for sub message length
            return startPosition;
        }

        public void EndSubMessage(int startPosition)
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            --depth;

            int length = position - startPosition;

            buffer[startPosition++] = (byte)((length) & 0xFF);
            buffer[startPosition++] = (byte)((length >> 8) & 0xFF);
            buffer[startPosition++] = (byte)((length >> 16) & 0xFF);
            buffer[startPosition++] = (byte)((length >> 24) & 0xFF);
        }

        private void PrivateWriteArray(byte[] value)
        {
            int length = value.Length;

            PrivateWriteVarint((uint)length);

            Ensure(length);

            Array.Copy(value, 0, this.buffer, this.position, length);
            this.position += length;
        }

        private void PrivateWriteFixed32(uint value)
        {
            Ensure(4);

            buffer[position++] = (byte)((value) & 0xFF);
            buffer[position++] = (byte)((value >> 8) & 0xFF);
            buffer[position++] = (byte)((value >> 16) & 0xFF);
            buffer[position++] = (byte)((value >> 24) & 0xFF);
        }

        private void PrivateWriteFixed64(ulong value)
        {
            Ensure(8);

            buffer[position++] = (byte)((value) & 0xFF);
            buffer[position++] = (byte)((value >> 8) & 0xFF);
            buffer[position++] = (byte)((value >> 16) & 0xFF);
            buffer[position++] = (byte)((value >> 24) & 0xFF);
            buffer[position++] = (byte)((value >> 32) & 0xFF);
            buffer[position++] = (byte)((value >> 40) & 0xFF);
            buffer[position++] = (byte)((value >> 48) & 0xFF);
            buffer[position++] = (byte)((value >> 56) & 0xFF);
        }

        private void PrivateWriteVarint(uint value)
        {
            Ensure(10);

            do
            {
                buffer[position++] = (byte)((value & 0x7F) | 0x80);
            } while ((value >>= 7) > 0);
            buffer[position - 1] &= 0x7F;
        }

        private void PrivateWriteVarint(ulong value)
        {
            Ensure(10);

            do
            {
                buffer[position++] = (byte)((value & 0x7F) | 0x80);
            } while ((value >>= 7) > 0);
            buffer[position - 1] &= 0x7F;
        }

        private void Ensure(int length)
        {
            int requiredSize = this.position + length;

            if (this.buffer.Length < requiredSize)
            {
                if (requiredSize < BufferPool.DefaultBufferSize)
                    requiredSize = BufferPool.DefaultBufferSize;

                this.buffer = BufferPool.Change(this.buffer, this.position, requiredSize);
            }
        }

        public void Dispose()
        {
#if DEBUG && !PocketPC
            Protocol.ProtoBuffers.Tools.VisualDebuging(depth, System.Reflection.MethodInfo.GetCurrentMethod());
#endif

            if (depth != 0)
            {
                throw new InvalidProgramException();
            }

            int length = this.position - Values.HeaderSize /* reserved space for header */;

            // message size
            buffer[Values.SizePosition + 0] = (byte)((length) & 0xFF);
            buffer[Values.SizePosition + 1] = (byte)((length >> 8) & 0xFF);
            buffer[Values.SizePosition + 2] = (byte)((length >> 16) & 0xFF);
            buffer[Values.SizePosition + 3] = (byte)((length >> 24) & 0xFF);

            // message data
            sourceStream.Write(this.buffer, 0, this.position);

            BufferPool.UnlockBuffer(this.buffer);
            this.buffer = null;
        }
    }
}
