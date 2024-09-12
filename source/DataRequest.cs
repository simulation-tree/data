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
        public readonly Entity entity;

        public readonly FixedString Address
        {
            get
            {
                IsDataRequest component = entity.GetComponentRef<IsDataRequest>();
                return component.address;
            }
        }

        public readonly USpan<byte> Data => entity.GetArray<byte>();

        readonly uint IEntity.Value => entity.value;
        readonly World IEntity.World => entity.world;
        readonly Definition IEntity.Definition => new Definition().AddComponentType<IsData>();

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

        public readonly override string ToString()
        {
            return Address.ToString();
        }

        public bool TryGetData(out USpan<byte> data)
        {
            if (this.IsCompliant())
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
    }
}
