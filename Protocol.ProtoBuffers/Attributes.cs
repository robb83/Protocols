using System;

namespace Protocol.ProtoBuffers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    public class ProtoContractAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ProtoMemberAttribute : Attribute
    {
        private int tag;
        public ProtoMemberAttribute(int tag)
        {
            this.tag = tag;
        }

        public int Tag { get { return this.tag; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ProtoIgnoreAttribute : Attribute { }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = true)]
    public class ProtoIncludeAttribute : Attribute 
    {
        public ProtoIncludeAttribute(int tag, Type type)
        {

        }        
    }
}
