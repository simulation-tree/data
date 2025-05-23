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
        Uninitialized,

        /// <summary>
        /// Request is submitted and waiting to be handled.
        /// </summary>
        Submitted,

        /// <summary>
        /// Request is active and is being processed.
        /// </summary>
        Loading,

        /// <summary>
        /// Request is complete and data has been found.
        /// </summary>
        Loaded,

        /// <summary>
        /// Request is complete but data was not found.
        /// </summary>
        NotFound,

        /// <summary>
        /// Data has been loaded and consumed by another action.
        /// </summary>
        Consumed
    }
}