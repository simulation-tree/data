using Simulation;
using Data.Components;
using Data.Events;
using System;
using Unmanaged;

namespace Data
{
    /// <summary>
    /// An entity that will contain data loaded from its address.
    /// </summary>
    public readonly struct DataRequest : IDataRequest, IDisposable
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

        public readonly bool IsLoaded => entity.ContainsList<byte>();
        public readonly ReadOnlySpan<byte> Bytes => entity.GetList<byte>().AsSpan();

        World IEntity.World => entity.world;
        eint IEntity.Value => entity.value;

#if NET
        [Obsolete("Default constructor not supported.", true)]
        public DataRequest()
        {
            throw new NotSupportedException();
        }
#endif

        public DataRequest(World world, eint existingEntity)
        {
            this.entity = new(world, existingEntity);
        }

        public DataRequest(World world, ReadOnlySpan<char> address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));

            world.Submit(new DataUpdate());
            world.Poll();
        }

        public DataRequest(World world, FixedString address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));

            world.Submit(new DataUpdate());
            world.Poll();
        }

        public readonly override string ToString()
        {
            return entity.GetComponent<IsDataRequest>().address.ToString();
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        Query IEntity.GetQuery(World world)
        {
            return new Query(world, RuntimeType.Get<IsDataRequest>());
        }
    }
}
