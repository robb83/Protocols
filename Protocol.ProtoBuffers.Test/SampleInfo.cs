using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Test
{
    public class SampleInfo
    {
        public readonly Type ExpectedType;
        public readonly object Source;

        public SampleInfo(Type expectedType, object source)
        {
            this.ExpectedType = expectedType;
            this.Source = source;
        }
    }
}
