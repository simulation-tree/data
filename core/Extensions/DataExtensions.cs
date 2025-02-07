using Data.Components;
using Unmanaged;
using Worlds;

namespace Data
{
    public static class DataExtensions
    {
        /// <summary>
        /// Retrieves the <see cref="BinaryData"/> array from the entity.
        /// </summary>
        public static USpan<byte> GetBytes<T>(this T entity) where T : unmanaged, IEntity
        {
            return entity.AsEntity().GetArray<BinaryData>().As<byte>();
        }
    }
}
