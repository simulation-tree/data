using System.Collections.Generic;
using System.Reflection;
using Unmanaged;

namespace Data
{
    public readonly struct EmbeddedAddress
    {
        private static readonly List<EmbeddedAddress> all = new();

        public static IReadOnlyList<EmbeddedAddress> All => all;

        public readonly Assembly assembly;
        public readonly Address address;

        public EmbeddedAddress(Assembly assembly, Address address)
        {
            this.assembly = assembly;
            this.address = address;
        }

        public static void Register<T>() where T : unmanaged, IEmbeddedResources
        {
            T resources = new();
            Assembly assembly = typeof(T).Assembly;
            foreach (Address address in resources.Addresses)
            {
                Register(assembly, address);
            }
        }

        public static void Register(Assembly assembly, FixedString address)
        {
            Register(assembly, new Address(address));
        }

        public static void Register(Assembly assembly, Address address)
        {
            all.Add(new EmbeddedAddress(assembly, address));
        }
    }
}
