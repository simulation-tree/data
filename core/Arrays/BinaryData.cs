namespace Data.Components
{
    /// <summary>
    /// A single <see cref="byte"/>.
    /// </summary>
    public struct BinaryData
    {
        private byte value;

        /// <summary>
        /// Creates a single <see cref="byte"/>.
        /// </summary>
        public BinaryData(byte value)
        {
            this.value = value;
        }
    }
}