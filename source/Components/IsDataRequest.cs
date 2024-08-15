using System;
using Unmanaged;

namespace Data.Components
{
    public struct IsDataRequest
    {
        public FixedString address;

        /// <summary>
        /// When <c>true</c> it implies that the address
        /// value has changed and the <see cref="byte"/> data
        /// should be reimported.
        /// </summary>
        public bool changed;

        public IsDataRequest(ReadOnlySpan<char> address)
        {
            this.address = new(address);
            changed = true;
        }

        public IsDataRequest(FixedString address)
        {
            this.address = address;
            changed = true;
        }
    }
}