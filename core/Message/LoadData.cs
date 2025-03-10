using System;
using System.Diagnostics;
using Unmanaged;
using Worlds;

namespace Data.Messages
{
    /// <summary>
    /// A message for loading binary data, that must be disposed after completing.
    /// </summary>
    public readonly struct LoadData : IDisposable
    {
        public readonly World world;
        public readonly Address address;

        private readonly ByteReader loadedData;

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

        private LoadData(World world, Address address, ByteReader loadedData)
        {
            this.world = world;
            this.address = address;
            this.loadedData = loadedData;
        }

        public readonly void Dispose()
        {
            ThrowIfNotLoaded();

            loadedData.Dispose();
        }

        public readonly LoadData BecomeLoaded(ByteReader loadedData)
        {
            ThrowIfAlreadyLoaded();

            return new LoadData(world, address, loadedData);
        }

        /// <summary>
        /// Tries to retrieve the loaded bytes.
        /// </summary>
        public readonly bool TryGetBytes(out ReadOnlySpan<byte> data)
        {
            if (!loadedData.IsDisposed)
            {
                data = loadedData.GetBytes();
                return true;
            }

            data = default;
            return false;
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