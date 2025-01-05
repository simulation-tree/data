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
    public readonly struct DataRequest : IEntity
    {
        private readonly Entity entity;

        /// <summary>
        /// Address that this data request is looking for.
        /// </summary>
        public readonly Address Address
        {
            get
            {
                ref IsDataRequest component = ref entity.GetComponent<IsDataRequest>();
                return component.address;
            }
        }

        public readonly USpan<byte> Data
        {
            get
            {
                ThrowIfDataNotAvailable();
                return entity.GetArray<BinaryData>().As<byte>();
            }
        }

        /// <summary>
        /// Checks if the data has been loaded.
        /// </summary>
        public readonly bool IsLoaded => this.Is();

        readonly uint IEntity.Value => entity.value;
        readonly World IEntity.World => entity.world;

        readonly Definition IEntity.GetDefinition(Schema schema)
        {
            return new Definition().AddComponentType<IsData>(schema);
        }

#if NET
        [Obsolete("Default constructor not supported", true)]
        public DataRequest()
        {
            throw new NotSupportedException();
        }
#endif

        public DataRequest(World world, uint existingEntity)
        {
            this.entity = new(world, existingEntity);
        }

        public DataRequest(World world, USpan<char> address)
        {
            entity = new Entity<IsDataRequest>(world, new IsDataRequest(address));
        }

        public DataRequest(World world, Address address)
        {
            entity = new Entity<IsDataRequest>(world, new IsDataRequest(address));
        }

        public DataRequest(World world, string address)
        {
            entity = new Entity<IsDataRequest>(world, new IsDataRequest(address));
        }

        public DataRequest(World world, IEnumerable<char> address)
        {
            entity = new Entity<IsDataRequest>(world, new IsDataRequest(address));
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        public readonly override string ToString()
        {
            return Address.ToString();
        }

        public readonly bool TryGetData(out USpan<byte> data)
        {
            if (this.Is())
            {
                data = entity.GetArray<BinaryData>().As<byte>();
                return true;
            }
            else
            {
                data = default;
                return false;
            }
        }

        public readonly void SetAddress(Address address)
        {
            ref IsDataRequest component = ref entity.GetComponent<IsDataRequest>();
            component.address = address;
            component.version++;
        }

        /// <summary>
        /// Reads the data as a UTF8 string.
        /// </summary>
        /// <returns>Amount of <c>char</c> values copied.</returns>
        public readonly uint CopyDataAsUTF8To(USpan<char> buffer)
        {
            ThrowIfDataNotAvailable();

            using BinaryReader reader = new(Data);
            return reader.ReadUTF8Span(buffer);
        }

        [Conditional("DEBUG")]
        public void ThrowIfDataNotAvailable()
        {
            if (!this.Is())
            {
                throw new InvalidOperationException($"Data not yet available on `{entity.GetEntityValue()}`");
            }
        }

        public static implicit operator Entity(DataRequest request)
        {
            return request.entity;
        }
    }
}
