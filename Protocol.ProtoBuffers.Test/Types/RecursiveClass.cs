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
    public class RecursiveClass
    {
        [ProtoMember(1)]
        public int IntValue;

        [ProtoMember(2)]
        public RecursiveClass RecusiveTag;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            RecursiveClass b = (RecursiveClass)obj;

            if (this.IntValue != b.IntValue)
                return false;

            if (!CompareHelper.IsEqualObject(this.RecusiveTag, b.RecusiveTag))
                return false;

            return true;
        }
    }
}
