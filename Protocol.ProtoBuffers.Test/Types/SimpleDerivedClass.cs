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
    public class SimpleDerivedClass : SimpleBaseClass
    {
        [ProtoMember(3)]
        public String StringValue1;

        [ProtoMember(4)]
        public int IntValue1;

        [ProtoMember(5)]
        public DateTime DateTime1;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            SimpleDerivedClass b = (SimpleDerivedClass)obj;

            if (this.IntValue1 != b.IntValue1)
                return false;

            if (!CompareHelper.IsEqual(this.StringValue1, b.StringValue1))
                return false;

            if (!CompareHelper.IsEqual(this.DateTime1, b.DateTime1))
                return false;

            return base.Equals(obj);
        }
    }
}
