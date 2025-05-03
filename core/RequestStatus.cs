namespace Data
{
    /// <summary>
    /// Describes the possible state of a request.
    /// </summary>
    public enum RequestStatus : byte
    {
        /// <summary>
        /// Uninitialized.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Request is submitted and waiting to be handled.
        /// </summary>
        Submitted = 1,

        /// <summary>
        /// Request is active and is being processed.
        /// </summary>
        Loading = 2,

        /// <summary>
        /// Request is complete and data has been found.
        /// </summary>
        Loaded = 3,

        /// <summary>
        /// Request is complete but data was not found.
        /// </summary>
        NotFound = 4
    }
}