using Collections;
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

        private readonly Array<byte> bytes;

        /// <summary>
        /// Checks if this message was completed.
        /// <para>
        /// If so, it must be disposed after reading its bytes.
        /// </para>
        /// </summary>
        public readonly bool IsLoaded => !bytes.IsDisposed;

        /// <summary>
        /// Loaded bytes.
        /// </summary>
        public readonly USpan<byte> Bytes
        {
            get
            {
                ThrowIfNotLoaded();

                return bytes.AsSpan();
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
            this.bytes = default;
        }

        /// <summary>
        /// Creates a message that requests for data from the given <paramref name="address"/>.
        /// <para>
        /// Once loaded, it must be disposed.
        /// </para>
        /// </summary>
        public LoadData(World world, FixedString address)
        {
            this.world = world;
            this.address = new(address);
            this.bytes = default;
        }

        private LoadData(World world, Address address, USpan<byte> bytes)
        {
            this.world = world;
            this.address = address;
            this.bytes = new(bytes);
        }

        public readonly LoadData BecomeLoaded(USpan<byte> bytes)
        {
            ThrowIfAlreadyLoaded();

            return new LoadData(world, address, bytes);
        }

        public readonly void Dispose()
        {
            ThrowIfNotLoaded();

            bytes.Dispose();
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