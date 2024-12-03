using System;
using System.Collections.Generic;
using System.Reflection;
using Unmanaged;

namespace Data
{
    public readonly struct EmbeddedAddress
    {
        private static readonly List<EmbeddedAddress> all = new();
        private static readonly HashSet<Address> addresses = new();

        public static IReadOnlyList<EmbeddedAddress> All => all;

        public readonly Assembly assembly;
        public readonly Address address;

        public EmbeddedAddress(Assembly assembly, Address address)
        {
            this.assembly = assembly;
            this.address = address;
        }

        public static void Register<T>() where T : unmanaged
        {
            T resources = new();
            Assembly assembly = typeof(T).Assembly;
            if (resources is IEmbeddedResources embeddedResources)
            {
                foreach (Address address in embeddedResources.Addresses)
                {
                    Register(assembly, address);
                }
            }
            else if (resources is IDataReference dataReference)
            {
                Register(assembly, dataReference.Value);
            }
            else
            {
                throw new NotImplementedException($"Handling of `{typeof(T).Name}` as an embedded resource is not implemented");
            }
        }

        public static void Register(Assembly assembly, FixedString address)
        {
            Register(assembly, new Address(address));
        }

        public static void Register(Assembly assembly, Address address)
        {
            if (addresses.Add(address))
            {
                all.Add(new EmbeddedAddress(assembly, address));
            }
        }
    }
}
