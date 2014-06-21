
namespace Protocol.ProtoBuffers
{
    public interface IProtoSerializer
    {
        object Read(ProtoReader reader);
        void Writer(ProtoWriter writer, object instance);
    }
}
