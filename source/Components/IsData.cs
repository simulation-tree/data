using System;
using Unmanaged;

namespace Data.Components
{
    public struct IsData
    {
        public FixedString address;

        public IsData(FixedString address)
        {
            this.address = address;
        }

        public IsData(ReadOnlySpan<char> address)
        {
            this.address = new(address);
        }
    }
}