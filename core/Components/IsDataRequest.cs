using System.Collections.Generic;
using Unmanaged;
using Worlds;

namespace Data.Components
{
    [Component]
    public struct IsDataRequest
    {
        public Address address;
        public uint version;

        public IsDataRequest(USpan<char> address)
        {
            version = default;
            this.address = new(address);
        }

        public IsDataRequest(Address address)
        {
            version = default;
            this.address = address;
        }

        public IsDataRequest(string address)
        {
            version = default;
            this.address = new(address);
        }

        public IsDataRequest(IEnumerable<char> address)
        {
            version = default;
            this.address = new(address);
        }
    }
}