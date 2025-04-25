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
        public readonly World world;
        public readonly Address address;

        private ByteReader loadedData;

        /// <summary>
        /// Checks if this message was completed.
        /// <para>
        /// If so, it must be disposed after reading its data.
        /// </para>
        /// </summary>
        public readonly bool IsLoaded => !loadedData.IsDisposed;

        /// <summary>
        /// Loaded bytes.
        /// </summary>
        public readonly ReadOnlySpan<byte> Bytes
        {
            get
            {
                ThrowIfNotLoaded();

                return loadedData.GetBytes();
            }
        }

        /// <summary>
        /// Creates a message that requests for data from the given <paramref name="address"/>.
        /// <para>
        /// Once loaded, it must be disposed.
        /// </para>
        /// </summary>
        public LoadData(World world, Address address)
        {
            this.world = world;
            this.address = address;
            this.loadedData = default;
        }

        /// <summary>
        /// Creates a message that requests for data from the given <paramref name="address"/>.
        /// <para>
        /// Once loaded, it must be disposed.
        /// </para>
        /// </summary>
        public LoadData(World world, ASCIIText256 address)
        {
            this.world = world;
            this.address = new(address);
            this.loadedData = default;
        }

        /// <summary>
        /// Tries to consume the data loaded in this message.
        /// <para>
        /// Disposing of the retrieved <paramref name="loadedData"/> is required.
        /// </para>
        /// </summary>
        public bool TryConsume(out ByteReader loadedData)
        {
            ThrowIfNotLoaded();

            bool isLoaded = !this.loadedData.IsDisposed;
            loadedData = this.loadedData;
            this.loadedData = default;
            return isLoaded;
        }

        public void BecomeLoaded(ByteReader loadedData)
        {
            ThrowIfAlreadyLoaded();

            this.loadedData = loadedData;
        }

        [Conditional("DEBUG")]
        private readonly void ThrowIfNotLoaded()
        {
            if (!IsLoaded)
            {
                throw new InvalidOperationException($"Data for `{address}` is not available");
            }
        }

        [Conditional("DEBUG")]
        private readonly void ThrowIfAlreadyLoaded()
        {
            if (IsLoaded)
            {
                throw new InvalidOperationException($"Data for `{address}` is already loaded");
            }
        }
    }
}