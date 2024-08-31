using Data.Components;
using Simulation;
using System;
using Unmanaged;

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

        public readonly ReadOnlySpan<byte> Data => entity.GetArray<byte>();

        World IEntity.World => entity;
        uint IEntity.Value => entity;

#if NET
        [Obsolete("Default constructor not supported.", true)]
        public DataRequest()
        {
            throw new NotSupportedException();
        }
#endif

        public DataRequest(World world, uint existingEntity)
        {
            this.entity = new(world, existingEntity);
        }

        public DataRequest(World world, ReadOnlySpan<char> address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));
        }

        public DataRequest(World world, FixedString address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));
        }

        public readonly override string ToString()
        {
            return Address.ToString();
        }

        readonly Query IEntity.GetQuery(World world)
        {
            return new Query(world, RuntimeType.Get<IsData>());
        }

        public bool TryGetData(out Span<byte> data)
        {
            if (this.Is())
            {
                data = entity.GetArray<byte>();
                return true;
            }
            else
            {
                data = default;
                return false;
            }
        }

        public static implicit operator Entity(DataRequest request)
        {
            return request.entity;
        }
    }
}
