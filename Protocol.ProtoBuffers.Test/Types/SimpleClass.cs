using Protocol.ProtoBuffers;
using Protocol.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoType.Examples
{
    [ProtoContract]
    public class SimpleClass
    {
        [ProtoMember(1)]
        public String StringValue;

        [ProtoMember(2)]
        public int IntValue;

        [ProtoMember(3)]
        public long LongValue;

        [ProtoMember(4)]
        public String[] StringArrayValue;

        [ProtoMember(5)]
        public int[] IntArrayValue;

        [ProtoMember(6)]
        public long[] LongArrayValue;

        [ProtoMember(7)]
        public List<String> StringListValue;

        [ProtoMember(8)]
        public DateTime DateTime;

        [ProtoMember(9)]
        public Nullable<int> NullableInt;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            SimpleClass b = (SimpleClass)obj;

            if (this.IntValue != b.IntValue)
                return false;

            if (this.LongValue != b.LongValue)
                return false;

            if (this.NullableInt != b.NullableInt)
                return false;

            if (!CompareHelper.IsEqual(this.LongArrayValue, b.LongArrayValue))
                return false;

            if (!CompareHelper.IsEqual(this.IntArrayValue, b.IntArrayValue))
                return false;

            if (!CompareHelper.IsEqual(this.StringValue, b.StringValue))
                return false;

            if (!CompareHelper.IsEqual(this.StringArrayValue, b.StringArrayValue))
                return false;

            if (!CompareHelper.IsEqual(this.StringListValue, b.StringListValue))
                return false;

            if (!CompareHelper.IsEqual(this.DateTime, b.DateTime))
                return false;

            return true;
        }
    }
}
