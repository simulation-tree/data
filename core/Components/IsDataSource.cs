using Unmanaged;
using Worlds;

namespace Data.Components
{
    [Component]
    public struct IsDataSource
    {
        public Address address;

        public IsDataSource(FixedString address)
        {
            this.address = new(address);
        }

        public IsDataSource(Address address)
        {
            this.address = address;
        }

        public IsDataSource(USpan<char> address)
        {
            this.address = new(address);
        }
    }
}