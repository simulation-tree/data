namespace Data.Components
{
    /// <summary>
    /// A single <see cref="byte"/>.
    /// </summary>
    public readonly struct DataByte
    {
        private readonly byte value;

        /// <summary>
        /// Creates a single <see cref="byte"/>.
        /// </summary>
        public DataByte(byte value)
        {
            this.value = value;
        }

        /// <inheritdoc/>
        public static implicit operator byte(DataByte dataByte)
        {
            return dataByte.value;
        }

        /// <inheritdoc/>
        public static implicit operator DataByte(byte value)
        {
            return new DataByte(value);
        }
    }
}