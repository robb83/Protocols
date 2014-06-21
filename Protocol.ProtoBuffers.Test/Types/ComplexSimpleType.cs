using Protocol.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.ProtoBuffers.Test
{
    [Protocol.ProtoBuffers.ProtoContract]
    public class ComplexSimpleType
    {
        [Protocol.ProtoBuffers.ProtoMember(1)]
        public double DoubleValue;
        [Protocol.ProtoBuffers.ProtoMember(2)]
        public ComplexSimpleNestedType[] NestedTypes;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            ComplexSimpleType b = (ComplexSimpleType)obj;

            if (this.DoubleValue != b.DoubleValue)
                return false;

            if (CompareHelper.IsEqualArray(this.NestedTypes, b.NestedTypes))
                return false;

            return true;
        }
    }
}
