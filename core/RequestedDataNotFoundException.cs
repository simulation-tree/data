using System;

namespace Data
{
    /// <summary>
    /// Thrown when requested data could not be found.
    /// </summary>
    public class RequestedDataNotFoundException : Exception
    {
        public RequestedDataNotFoundException(string message) : base(message)
        {
        }
    }
}
