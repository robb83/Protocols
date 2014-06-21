using System;
using System.Collections.Generic;
using System.Threading;

/*
 * https://github.com/mgravell/protobuf-net/blob/master/protobuf-net/BufferPool.cs
 */

namespace Protocol.ProtoBuffers
{
    public static class BufferPool
    {
        private const int DefaultPoolSize = 20;
        private static object[] freeBuffers = new object[DefaultPoolSize];
        public const int DefaultBufferSize = 1024; // 256kb

        public static byte[] Change(byte[] buffer, int contentLength, int requiredSize)
        {
            int buffer_size = buffer.Length * 2;
            if (buffer_size < requiredSize)
                buffer_size = requiredSize;

            byte[] resizedBuffer = new byte[buffer_size];

            Array.Copy(buffer, 0, resizedBuffer, 0, contentLength);

            UnlockBuffer(buffer);

            return resizedBuffer;
        }

        public static byte[] LockBuffer()
        {
            object tmp;

            for (int i = 0; i < DefaultPoolSize; i++)
            {
                if ((tmp = Interlocked.Exchange(ref freeBuffers[i], null)) != null)
                    return (byte[])tmp;
            }

            return new byte[DefaultBufferSize];
        }

        public static void UnlockBuffer(byte[] buffer)
        {
            if (buffer == null)
                return;
            if (buffer.Length != DefaultBufferSize)
                return;

            for (int i = 0; i < DefaultPoolSize; i++)
            {
                if (Interlocked.CompareExchange(ref freeBuffers[i], buffer, null) == null)
                {
                    break; // found a null; swapped it in
                }
            }
        }
    }
}
