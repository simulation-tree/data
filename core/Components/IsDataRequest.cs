using System;
using System.Collections.Generic;
using Unmanaged;

namespace Data.Components
{
    public struct IsDataRequest
    {
        public readonly Address address;
        public RequestStatus status;
        public TimeSpan timeout;

        public IsDataRequest(USpan<char> address, RequestStatus status, TimeSpan timeout)
        {
            this.address = new(address);
            this.status = status;
            this.timeout = timeout;
        }

        public IsDataRequest(Address address, RequestStatus status, TimeSpan timeout)
        {
            this.address = address;
            this.status = status;
            this.timeout = timeout;
        }

        public IsDataRequest(string address, RequestStatus status, TimeSpan timeout)
        {
            this.address = new(address);
            this.status = status;
            this.timeout = timeout;
        }

        public IsDataRequest(IEnumerable<char> address, RequestStatus status, TimeSpan timeout)
        {
            this.address = new(address);
            this.status = status;
            this.timeout = timeout;
        }

        public readonly IsDataRequest BecomeLoaded()
        {
            return new(address, RequestStatus.Loaded, timeout);
        }
    }
}