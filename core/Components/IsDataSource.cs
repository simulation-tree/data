using System;
using Unmanaged;

namespace Data.Components
{
    public struct IsDataSource
    {
        public Address address;

        public IsDataSource(ASCIIText256 address)
        {
            this.address = new(address);
        }

        public IsDataSource(Address address)
        {
            this.address = address;
        }

        public IsDataSource(Span<char> address)
        {
            this.address = new(address);
        }
    }
}