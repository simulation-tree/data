using System.Collections.Generic;
using System.Reflection;

namespace Data
{
    /// <summary>
    /// Registry containing all known embedded resources.
    /// </summary>
    public static class EmbeddedResourceRegistry
    {
        private static readonly List<EmbeddedResource> all = new();

        public static IReadOnlyList<EmbeddedResource> All => all;

        public static void Load<T>() where T : unmanaged, IEmbeddedResourcesBank
        {
            T template = default;
            Assembly assembly = typeof(T).Assembly;
            template.Load(new(Register));

            void Register(Address address)
            {
                EmbeddedResourceRegistry.Register(assembly, address);
            }
        }

        public static void Register(Assembly assembly, Address address)
        {
            all.Add(new EmbeddedResource(assembly, address));
        }

        private static bool TryGetMatch(Address address, out int index)
        {
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i].address.Matches(address))
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }

        public static bool Contains(Address address)
        {
            return TryGetMatch(address, out _);
        }

        public static bool TryGet(Address address, out EmbeddedResource resource)
        {
            if (TryGetMatch(address, out int index))
            {
                resource = all[index];
                return true;
            }

            resource = default;
            return false;
        }

        public static EmbeddedResource Get(Address address)
        {
            TryGetMatch(address, out int index);
            return all[index];
        }
    }
}