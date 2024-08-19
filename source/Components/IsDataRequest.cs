using System;
using Unmanaged;

namespace Data.Components
{
    public struct IsDataRequest
    {
        public FixedString address;
        public DataRequest.DataStatus status;

        public IsDataRequest(ReadOnlySpan<char> address)
        {
            this.address = new(address);
            status = DataRequest.DataStatus.Unknown;
        }

        public IsDataRequest(FixedString address)
        {
            this.address = address;
            status = DataRequest.DataStatus.Unknown;
        }
    }
}