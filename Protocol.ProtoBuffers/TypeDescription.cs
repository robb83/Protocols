using System;
using System.Collections.Generic;
using System.Threading;

namespace Protocol.ProtoBuffers.Serializer
{
    public class TypeDescription
    {
        private Type descriptionForType;
        private volatile bool pending;
        private volatile bool supported;
        private IProtoSerializer serializerForMessage;
        private IProtoSerializer serializerForNestedMessage;
        private LinkedList<TypeDescription> depends;

        public TypeDescription(Type descriptionForType)
        {
            this.supported = false;
            this.pending = true;
            this.descriptionForType = descriptionForType;
            this.depends = new LinkedList<TypeDescription>();
        }

        public TypeDescription(Type descriptionForType, IProtoSerializer messageSerializer, IProtoSerializer nestedMessageSerializer)
        {
            this.pending = false;
            this.descriptionForType = descriptionForType;
            this.serializerForMessage = messageSerializer;
            this.serializerForNestedMessage = nestedMessageSerializer;
            this.supported = (messageSerializer != null && nestedMessageSerializer != null);
        }

        public bool Pending { get { return this.pending; } }

        public bool Supported { get { return this.supported; } }

        public IProtoSerializer MessageSerializer { get { return this.serializerForMessage; } }

        public IProtoSerializer NestedMessageSerializer { get { return this.serializerForNestedMessage; } }

        public void AddDepends(TypeDescription description)
        {
            if (!this.pending)
                throw new InvalidProgramException();

            if (!this.depends.Contains(description))
                this.depends.AddLast(description);
        }

        public void Complete(IProtoSerializer messageSerializer, IProtoSerializer nestedMessageSerializer)
        {
            if (!this.pending)
                return;

            this.serializerForMessage = messageSerializer;
            this.serializerForNestedMessage = nestedMessageSerializer;
            this.supported = (messageSerializer != null && nestedMessageSerializer != null);
            this.pending = false;
        }
    }
}
