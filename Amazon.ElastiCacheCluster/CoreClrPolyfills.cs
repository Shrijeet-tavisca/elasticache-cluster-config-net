#if NETSTANDARD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO
{
    internal static class MemoryStreamExtensions
    {
        public static byte[] GetBuffer(this MemoryStream stream)
        {
            ArraySegment<byte> result;
            if (!stream.TryGetBuffer(out result))
            {
                throw new InvalidOperationException("Buffer not available.");
            }
            return result.Array;
        }
    }
}
#endif
