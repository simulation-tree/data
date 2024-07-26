using Simulation;
using Data.Components;
using Data.Events;
using System;
using Unmanaged;

namespace Data
{
    /// <summary>
    /// An entity that contains the found data for the 
    /// address in the request.
    /// </summary>
    public readonly struct DataRequest : IEntity, IDisposable
    {
        public readonly Entity entity;

        public readonly FixedString Address
        {
            get => entity.GetComponent<Entity, IsDataRequest>().address;
            set
            {
                ref IsDataRequest data = ref entity.GetComponentRef<Entity, IsDataRequest>();
                if (data.address != value)
                {
                    data.address = value;
                    data.changed = true;
                }
            }
        }

        World IEntity.World => entity.world;
        eint IEntity.Value => entity.value;

#if NET5_0_OR_GREATER
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

        public readonly override string ToString()
        {
            return Address.ToString();
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        /// <summary>
        /// Returns the loaded bytes as a span.
        /// </summary>
        public readonly ReadOnlySpan<byte> AsSpan()
        {
            if (!entity.ContainsList<Entity, byte>())
            {
                throw new NullReferenceException($"Entity {entity} with request {Address} does not contain any data.");
            }

            return entity.GetList<Entity, byte>().AsSpan();
        }
        
        public static Query GetQuery(World world)
        {
            return new Query(world, RuntimeType.Get<IsDataRequest>());
        }
    }
}
