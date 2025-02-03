using Data.Components;
using Unmanaged;
using Worlds;

namespace Data.Messages
{
    public readonly struct HandleDataRequest
    {
        public readonly Entity entity;
        public readonly FixedString address;
        public readonly bool loaded;

        public readonly USpan<byte> Bytes => entity.GetArray<BinaryData>().As<byte>();

        public HandleDataRequest(Entity entity, FixedString address)
        {
            this.entity = entity;
            this.address = address;
        }

        public HandleDataRequest(Entity entity, FixedString address, bool loaded)
        {
            this.entity = entity;
            this.address = address;
            this.loaded = loaded;
        }

        public readonly HandleDataRequest BecomeLoaded()
        {
            return new HandleDataRequest(entity, address, true);
        }
    }
}