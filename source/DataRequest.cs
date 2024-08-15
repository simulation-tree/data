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
            return entity.GetComponent<Entity, IsDataRequest>().address.ToString();
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        Query IEntity.GetQuery(World world)
        {
            return new Query(world, RuntimeType.Get<IsDataRequest>());
        }

        public static FixedString GetAddress<T>() where T : unmanaged, IDataReference
        {
            return default(T).Value;
        }
    }

    public readonly struct DataRequest<T> : IDataRequest, IDisposable where T : unmanaged, IDataReference
    {
        private readonly DataRequest entity;

        World IEntity.World => entity.GetWorld();
        eint IEntity.Value => entity.GetEntityValue();

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

        public DataRequest(World world)
        {
            FixedString address = DataRequest.GetAddress<T>();
            entity = new(world, address);
        }

        public readonly override string ToString()
        {
            return entity.ToString();
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
