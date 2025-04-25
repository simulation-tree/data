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
    public readonly partial struct DataRequest : IEntity
    {
        /// <summary>
        /// Address of the data request.
        /// </summary>
        public readonly Address Address => GetComponent<IsDataRequest>().address;

        /// <summary>
        /// Checks if this request is loaded.
        /// </summary>
        public readonly bool IsLoaded => GetComponent<IsDataRequest>().status == RequestStatus.Loaded;

        /// <summary>
        /// Bytes of the loaded data.
        /// </summary>
        public readonly ReadOnlySpan<byte> Bytes
        {
            get
            {
                ThrowIfNotLoaded();

                return GetArray<BinaryData>().AsSpan<byte>();
            }
        }

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<IsDataRequest>();
            archetype.AddArrayType<BinaryData>();
        }

        /// <summary>
        /// Creates a new data request entity.
        /// </summary>
        public DataRequest(World world, ReadOnlySpan<char> address, TimeSpan timeout = default)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataRequest(address, RequestStatus.Awaiting, timeout));
        }

        /// <summary>
        /// Creates a new data request entity.
        /// </summary>
        public DataRequest(World world, Address address, TimeSpan timeout = default)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataRequest(address, RequestStatus.Awaiting, timeout));
        }

        /// <summary>
        /// Creates a new data request entity.
        /// </summary>
        public DataRequest(World world, string address, TimeSpan timeout = default)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataRequest(address, RequestStatus.Awaiting, timeout));
        }

        /// <summary>
        /// Creates a new data request entity.
        /// </summary>
        public DataRequest(World world, IEnumerable<char> address, TimeSpan timeout = default)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataRequest(address, RequestStatus.Awaiting, timeout));
        }

        /// <inheritdoc/>
        public readonly override string ToString()
        {
            return Address.ToString();
        }

        /// <summary>
        /// Tries to retrieve the data if its loaded.
        /// </summary>
        public readonly bool TryGetData(out ReadOnlySpan<byte> data)
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

        /// <summary>
        /// Creates a new byte reader from the loaded bytes.
        /// </summary>
        /// <returns></returns>
        public readonly ByteReader CreateByteReader()
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