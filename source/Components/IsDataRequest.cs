using Unmanaged;
using Worlds;

namespace Data.Components
{
    [Component]
    public struct IsDataRequest
    {
        public FixedString address;
        public uint version;

        public IsDataRequest(USpan<char> address)
        {
            version = default;
            this.address = new(address);
        }

        public IsDataRequest(FixedString address)
        {
            version = default;
            this.address = address;
        }
    }
}