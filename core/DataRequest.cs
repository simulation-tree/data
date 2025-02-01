using Data.Components;
using System;
using System.Collections.Generic;
using Unmanaged;
using Worlds;

namespace Data
{
    /// <summary>
    /// An entity that will contain data loaded from its address.
    /// </summary>
    public readonly struct DataRequest : IDataRequest
    {
        private readonly Entity entity;

        readonly uint IEntity.Value => entity.value;
        readonly World IEntity.World => entity.world;

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<IsDataRequest>();
        }

#if NET
        [Obsolete("Default constructor not supported", true)]
        public DataRequest()
        {
            throw new NotSupportedException();
        }
#endif

        public DataRequest(World world, USpan<char> address, TimeSpan timeout = default)
        {
            entity = new Entity<IsDataRequest>(world, new IsDataRequest(address, RequestStatus.Submitted, timeout));
        }

        public DataRequest(World world, Address address, TimeSpan timeout = default)
        {
            entity = new Entity<IsDataRequest>(world, new IsDataRequest(address, RequestStatus.Submitted, timeout));
        }

        public DataRequest(World world, string address, TimeSpan timeout = default)
        {
            entity = new Entity<IsDataRequest>(world, new IsDataRequest(address, RequestStatus.Submitted, timeout));
        }

        public DataRequest(World world, IEnumerable<char> address, TimeSpan timeout = default)
        {
            entity = new Entity<IsDataRequest>(world, new IsDataRequest(address, RequestStatus.Submitted, timeout));
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        public readonly override string ToString()
        {
            return this.GetRequestAddress().ToString();
        }

        public readonly bool TryGetData(out USpan<byte> data)
        {
            if (this.IsLoaded())
            {
                data = this.GetBytes();
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