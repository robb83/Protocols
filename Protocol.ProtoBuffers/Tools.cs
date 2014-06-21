using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Protocol.ProtoBuffers
{
    public static class Tools
    {
        [Conditional("DEBUG")]
        internal static void VisualDebuging(int depth, MethodBase methodInfo)
        {
            System.Diagnostics.Debug.WriteLine("".PadLeft(depth) + methodInfo);
        }
    }
}
