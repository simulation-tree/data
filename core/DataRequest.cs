using Data.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unmanaged;
using Worlds;

namespace Data
{
    /// <summary>
    /// An entity that will contain data loaded from its address.
    /// </summary>
    public readonly partial struct DataRequest : IDataRequest
    {
        public readonly Address Address => GetComponent<IsDataRequest>().address;
        public readonly bool IsLoaded => GetComponent<IsDataRequest>().status == RequestStatus.Loaded;

        public readonly USpan<byte> Bytes
        {
            get
            {
                ThrowIfNotLoaded();

                return GetArray<BinaryData>().As<byte>();
            }
        }

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<IsDataRequest>();
            archetype.AddArrayType<BinaryData>();
        }

        public DataRequest(World world, USpan<char> address, TimeSpan timeout = default)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataRequest(address, RequestStatus.Submitted, timeout));
        }

        public DataRequest(World world, Address address, TimeSpan timeout = default)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataRequest(address, RequestStatus.Submitted, timeout));
        }

        public DataRequest(World world, string address, TimeSpan timeout = default)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataRequest(address, RequestStatus.Submitted, timeout));
        }

        public DataRequest(World world, IEnumerable<char> address, TimeSpan timeout = default)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataRequest(address, RequestStatus.Submitted, timeout));
        }

        public readonly override string ToString()
        {
            return Address.ToString();
        }

        public readonly bool TryGetData(out USpan<byte> data)
        {
            if (IsLoaded)
            {
                data = Bytes;
                return true;
            }
            else
            {
                data = default;
                return false;
            }
        }

        public readonly BinaryReader CreateBinaryReader()
        {
            ThrowIfNotLoaded();

            return new(Bytes);
        }

        [Conditional("DEBUG")]
        private readonly void ThrowIfNotLoaded()
        {
            if (!IsLoaded)
            {
                throw new InvalidOperationException($"Data entity `{value}` at address `{Address}` has not been loaded");
            }
        }
    }
}