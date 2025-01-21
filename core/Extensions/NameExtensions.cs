using Data.Components;
using Unmanaged;
using Worlds;

namespace Data
{
    public static class NameExtensions
    {
        public static ref FixedString GetName<T>(this T entity) where T : unmanaged, IEntity
        {
            return ref entity.AsEntity().GetComponent<Name>().value;
        }
    }
}
