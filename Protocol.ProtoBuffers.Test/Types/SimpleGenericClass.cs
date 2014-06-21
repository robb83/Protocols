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
    public class SimpleGenericClass<T>
    {
        [ProtoMember(1)]
        public int IntValue;

        [ProtoMember(2)]
        public T GenericValue;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            SimpleGenericClass<T> b = (SimpleGenericClass<T>)obj;

            if (this.IntValue != b.IntValue)
                return false;

            if (!CompareHelper.IsEqualObject(this.GenericValue, b.GenericValue))
                return false;

            return true;
        }
    }
}
