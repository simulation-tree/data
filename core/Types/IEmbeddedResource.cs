namespace Data
{
    /// <summary>
    /// Describes an <see cref="EmbeddedResource"/> that is expected to be found at runtime.
    /// </summary>
    public interface IEmbeddedResource
    {
        /// <summary>
        /// The address of the resource.
        /// </summary>
        Address Address { get; }
    }
}