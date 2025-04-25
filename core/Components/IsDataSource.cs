using System;
using Unmanaged;

namespace Data.Components
{
    /// <summary>
    /// A component indicating that the entity contains addressable data.
    /// </summary>
    public struct IsDataSource
    {
        /// <summary>
        /// The real address of the data.
        /// </summary>
        public Address address;

        /// <summary>
        /// Creates the component.
        /// </summary>
        public IsDataSource(ASCIIText256 address)
        {
            this.address = new(address);
        }

        /// <summary>
        /// Creates the component.
        /// </summary>
        public IsDataSource(Address address)
        {
            this.address = address;
        }

        /// <summary>
        /// Creates the component.
        /// </summary>
        public IsDataSource(Span<char> address)
        {
            this.address = new(address);
        }
    }
}