using Data.Functions;

namespace Data
{
    /// <summary>
    /// A bank containing multiple embedded resources.
    /// </summary>
    public interface IEmbeddedResourceBank
    {
        /// <summary>
        /// Loads embedded resources with the given <paramref name="register"/> function.
        /// </summary>
        void Load(Register register);
    }
}
