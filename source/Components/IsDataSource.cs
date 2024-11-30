using Unmanaged;
using Worlds;

namespace Data.Components
{
    [Component]
    public struct IsDataSource
    {
        public FixedString address;

        public IsDataSource(FixedString address)
        {
            this.address = address;
        }

        public IsDataSource(USpan<char> address)
        {
            this.address = new(address);
        }
    }
}