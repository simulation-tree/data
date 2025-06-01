namespace Data.Messages
{
    /// <summary>
    /// Message for updating only data systems.
    /// </summary>
    public readonly struct DataUpdate
    {
        /// <summary>
        /// Time passed.
        /// </summary>
        public readonly double deltaTime;

        /// <summary>
        /// Initializes the message.
        /// </summary>
        public DataUpdate(double deltaTime)
        {
            this.deltaTime = deltaTime;
        }
    }
}