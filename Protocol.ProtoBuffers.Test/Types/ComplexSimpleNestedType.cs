using Protocol.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.ProtoBuffers.Test
{
    [Protocol.ProtoBuffers.ProtoContract]
    public class ComplexSimpleNestedType
    {
        [Protocol.ProtoBuffers.ProtoMember(1)]
        public String StringValue;
        [Protocol.ProtoBuffers.ProtoMember(2)]
        public ComplexSimpleType ComplexSimple;
        [Protocol.ProtoBuffers.ProtoMember(3)]
        public int Int32Value;
        [Protocol.ProtoBuffers.ProtoMember(4)]
        public decimal DecimalValue;
        [Protocol.ProtoBuffers.ProtoMember(5)]
        public Guid GuidValue;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            ComplexSimpleNestedType b = (ComplexSimpleNestedType)obj;

            if (this.GuidValue != b.GuidValue)
                return false;

            if (this.Int32Value != b.Int32Value)
                return false;

            if (CompareHelper.IsEqual(this.StringValue, b.StringValue))
                return false;

            if (this.DecimalValue != b.DecimalValue)
                return false;

            if (CompareHelper.IsEqualObject(this.ComplexSimple, b.ComplexSimple))
                return false;

            return true;
        }
    }
}
