using Data.Components;
using Simulation;
using System;
using System.Threading.Tasks;
using System.Threading;
using Unmanaged;
using Unmanaged.Collections;

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

        public readonly DataStatus Status => entity.GetComponent<IsDataRequest>().status;
        public readonly ReadOnlySpan<byte> Data => entity.GetList<byte>().AsSpan();
        public readonly bool IsLoaded => Status == DataStatus.Loaded;
        public readonly bool IsFinished => Status == DataStatus.Loaded || Status == DataStatus.None;

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

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        readonly Query IEntity.GetQuery(World world)
        {
            return new Query(world, RuntimeType.Get<IsDataRequest>());
        }

        /// <summary>
        /// Awaits until the requested byte data has been loaded and available.
        /// </summary>
        public async Task UntilLoaded(CancellationToken cancellation = default)
        {
            while (!IsLoaded)
            {
                await Task.Delay(1, cancellation);
            }
        }

        public bool TryGetData(out UnmanagedList<byte> data)
        {
            if (IsLoaded)
            {
                data = entity.GetList<byte>();
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

        public enum DataStatus : byte
        {
            Unknown,
            Loading,
            Loaded,
            None
        }
    }
}
