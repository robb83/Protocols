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
    public class SimpleBaseClass
    {
        [ProtoMember(1)]
        public String StringValue;

        [ProtoMember(2)]
        public int IntValue;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            SimpleBaseClass b = (SimpleBaseClass)obj;

            if (this.IntValue != b.IntValue)
                return false;

            if (!CompareHelper.IsEqual(this.StringValue, b.StringValue))
                return false;

            return true;
        }
    }
}
