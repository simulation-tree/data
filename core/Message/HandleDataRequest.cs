using Data.Components;
using Unmanaged;
using Worlds;

namespace Data.Messages
{
    public struct HandleDataRequest
    {
        public readonly Entity entity;
        public IsDataRequest request;

        public readonly USpan<byte> Bytes => entity.GetArray<BinaryData>().As<byte>();

        public HandleDataRequest(Entity entity, IsDataRequest request)
        {
            this.entity = entity;
            this.request = request;
        }
    }
}