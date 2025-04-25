using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Data
{
    /// <summary>
    /// Registry containing all known <see cref="EmbeddedResource"/>s.
    /// </summary>
    public static class EmbeddedResourceRegistry
    {
        private static readonly List<EmbeddedResource> all = new();
        private static readonly List<Address> addresses = new();

        /// <summary>
        /// List of all registered embedded resources.
        /// </summary>
        public static IReadOnlyList<EmbeddedResource> All => all;

        /// <summary>
        /// Loads a bank of embedded resources.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Load<T>() where T : unmanaged, IEmbeddedResourceBank
        {
            T template = default;
            Assembly assembly = typeof(T).Assembly;
            template.Load(new(Register));

            void Register(Address address)
            {
                EmbeddedResourceRegistry.Register(assembly, address);
            }
        }

        /// <summary>
        /// Registers a single embedded resource.
        /// </summary>
        public static void Register(Assembly assembly, Address address)
        {
            all.Add(new EmbeddedResource(assembly, address));
            addresses.Add(address);
        }

        /// <summary>
        /// Tries to find the index of the given <paramref name="address"/> in the registry.
        /// </summary>
        private static bool TryGetMatch(Address address, out int index)
        {
            for (int i = 0; i < addresses.Count; i++)
            {
                if (addresses[i].Matches(address))
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }

        /// <summary>
        /// Checks if the registry contains the given <paramref name="address"/>.
        /// </summary>
        public static bool Contains(Address address)
        {
            return TryGetMatch(address, out _);
        }

        /// <summary>
        /// Tries to retrieve the embedded resource at the given <paramref name="address"/>.
        /// </summary>
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

        /// <summary>
        /// Retrieves the embedded resource at the given <paramref name="address"/>.
        /// </summary>
        public static EmbeddedResource Get(Address address)
        {
            ThrowIfMissing(address);

            TryGetMatch(address, out int index);
            return all[index];
        }

        /// <summary>
        /// Retrieves the embedded resource found in the given <typeparamref name="T"/> type.
        /// </summary>
        public static EmbeddedResource Get<T>() where T : unmanaged, IEmbeddedResource
        {
            T template = default;
            Address address = template.Address;
            ThrowIfMissing(address);

            return Get(address);
        }

        [Conditional("DEBUG")]
        private static void ThrowIfMissing(Address address)
        {
            if (!Contains(address))
            {
                throw new KeyNotFoundException($"Embedded resource at address `{address}` could not be found");
            }
        }
    }
}