using System;
using Unmanaged;

namespace Data.Components
{
    public struct IsDataSource
    {
        public FixedString address;

        public IsDataSource(FixedString address)
        {
            this.address = address;
        }

        public IsDataSource(ReadOnlySpan<char> address)
        {
            this.address = new(address);
        }
    }
}