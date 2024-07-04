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
    public readonly struct DataRequest : IDisposable
    {
        public readonly Entity entity;

        public readonly FixedString Address
        {
            get => entity.GetComponent<IsDataRequest>().address;
            set
            {
                ref IsDataRequest data = ref entity.GetComponentRef<IsDataRequest>();
                if (data.address != value)
                {
                    data.address = value;
                    data.changed = true;
                }
            }
        }

        public DataRequest()
        {
            throw new InvalidOperationException("Cannot create a request entity without a world.");
        }

        public DataRequest(World world, EntityID existingEntity)
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
            if (!entity.ContainsCollection<byte>())
            {
                throw new NullReferenceException($"Entity {entity} with request {Address} does not contain any data.");
            }

            return entity.GetCollection<byte>().AsSpan();
        }
    }
}
