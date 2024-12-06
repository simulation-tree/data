using Data.Components;
using System;
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

        public readonly FixedString Address
        {
            get
            {
                IsDataRequest component = entity.GetComponent<IsDataRequest>();
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

        readonly uint IEntity.Value => entity.value;
        readonly World IEntity.World => entity.world;
        readonly Definition IEntity.Definition => new Definition().AddComponentType<IsData>();

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
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));
        }

        public DataRequest(World world, FixedString address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        public readonly override string ToString()
        {
            return Address.ToString();
        }

        public bool TryGetData(out USpan<byte> data)
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

        /// <summary>
        /// Reads the data as a UTF8 string.
        /// </summary>
        /// <returns>Amount of <c>char</c> values copied.</returns>
        public uint CopyDataAsUTF8To(USpan<char> buffer)
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
