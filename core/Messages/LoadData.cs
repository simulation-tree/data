using System;
using System.Diagnostics;
using Unmanaged;
using Worlds;

namespace Data.Messages
{
    /// <summary>
    /// A message for loading binary data, that must be disposed after completing.
    /// </summary>
    public struct LoadData
    {
        /// <summary>
        /// The world being used to find the data source.
        /// </summary>
        public readonly World world;

        /// <summary>
        /// The address of the data being requested.
        /// </summary>
        public readonly Address address;

        /// <summary>
        /// Status of the request.
        /// </summary>
        public RequestStatus status;

        private ByteReader data;

        /// <summary>
        /// Loaded bytes.
        /// </summary>
        public readonly ReadOnlySpan<byte> Bytes
        {
            get
            {
                ThrowIfNotLoaded();

                return data.GetBytes();
            }
        }

        /// <summary>
        /// Creates a message that requests for data from the given <paramref name="address"/>.
        /// </summary>
        public LoadData(World world, Address address)
        {
            this.world = world;
            this.address = address;
            data = default;
            status = RequestStatus.Uninitialized;
        }

        /// <summary>
        /// Creates a message that requests for data from the given <paramref name="address"/>.
        /// </summary>
        public LoadData(World world, ASCIIText256 address)
        {
            this.world = world;
            this.address = new(address);
            data = default;
            status = RequestStatus.Uninitialized;
        }

        /// <summary>
        /// Tries to consume the data loaded in this message.
        /// <para>
        /// Disposing of the retrieved <paramref name="loadedData"/> is required.
        /// </para>
        /// </summary>
        public bool TryConsume(out ByteReader loadedData)
        {
            if (status == RequestStatus.Loaded)
            {
                status = RequestStatus.Consumed;
                loadedData = data;
                data = default;
                return true;
            }
            else
            {
                loadedData = default;
                return false;
            }
        }

        /// <summary>
        /// Marks this message as loaded.
        /// </summary>
        public void Load(ByteReader byteReader)
        {
            ThrowIfLoaded();

            data = byteReader;
            status = RequestStatus.Loaded;
        }

        /// <summary>
        /// Marks this message as completed, but not found.
        /// </summary>
        public void NotFound()
        {
            status = RequestStatus.NotFound;
        }

        [Conditional("DEBUG")]
        private readonly void ThrowIfNotLoaded()
        {
            if (status != RequestStatus.Loaded)
            {
                throw new InvalidOperationException($"Data for `{address}` is not available");
            }
        }

        [Conditional("DEBUG")]
        private readonly void ThrowIfLoaded()
        {
            if (status == RequestStatus.Loaded)
            {
                throw new InvalidOperationException($"Data for `{address}` is already loaded");
            }
        }
    }
}