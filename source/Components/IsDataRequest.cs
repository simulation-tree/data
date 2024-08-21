using System;
using Unmanaged;

namespace Data.Components
{
    public struct IsDataRequest
    {
        public FixedString address;
        public uint version;

        public IsDataRequest(ReadOnlySpan<char> address)
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