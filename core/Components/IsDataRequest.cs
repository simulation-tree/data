using System;
using System.Collections.Generic;

namespace Data.Components
{
    /// <summary>
    /// A component signifying that the entity is looking for data.
    /// </summary>
    public struct IsDataRequest
    {
        /// <summary>
        /// The address where the data should be found.
        /// </summary>
        public Address address;

        /// <summary>
        /// Status of the request.
        /// </summary>
        public RequestStatus status;

        /// <summary>
        /// Amount of time to wait until the request is considered failed.
        /// </summary>
        public TimeSpan timeout;

        /// <summary>
        /// Creates the component.
        /// </summary>
        public IsDataRequest(ReadOnlySpan<char> address, RequestStatus status, TimeSpan timeout)
        {
            this.address = new(address);
            this.status = status;
            this.timeout = timeout;
        }

        /// <summary>
        /// Creates the component.
        /// </summary>
        public IsDataRequest(Address address, RequestStatus status, TimeSpan timeout)
        {
            this.address = address;
            this.status = status;
            this.timeout = timeout;
        }

        /// <summary>
        /// Creates the component.
        /// </summary>
        public IsDataRequest(string address, RequestStatus status, TimeSpan timeout)
        {
            this.address = new(address);
            this.status = status;
            this.timeout = timeout;
        }

        /// <summary>
        /// Creates the component.
        /// </summary>
        public IsDataRequest(IEnumerable<char> address, RequestStatus status, TimeSpan timeout)
        {
            this.address = new(address);
            this.status = status;
            this.timeout = timeout;
        }
    }
}