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

        private Status status;
        private ByteReader data;

        /// <summary>
        /// Checks if the data has been found.
        /// </summary>
        public readonly bool IsFound => status == Status.Found;

        /// <summary>
        /// Checks if the data has been found and consumed by a handler.
        /// </summary>
        public readonly bool IsConsumed => status == Status.FoundAndConsumed;

        /// <summary>
        /// Loaded bytes.
        /// </summary>
        public readonly ReadOnlySpan<byte> Bytes
        {
            get
            {
                ThrowIfNotFound();

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
            status = Status.NotFound;
        }

        /// <summary>
        /// Creates a message that requests for data from the given <paramref name="address"/>.
        /// </summary>
        public LoadData(World world, ASCIIText256 address)
        {
            this.world = world;
            this.address = new(address);
            data = default;
            status = Status.NotFound;
        }

        /// <summary>
        /// Tries to consume the data loaded in this message.
        /// <para>
        /// Disposing of the retrieved <paramref name="loadedData"/> is required.
        /// </para>
        /// </summary>
        public bool TryConsume(out ByteReader loadedData)
        {
            if (status == Status.Found)
            {
                status = Status.FoundAndConsumed;
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
        public void Found(ByteReader byteReader)
        {
            ThrowIfFound();

            data = byteReader;
            status = Status.Found;
        }

        /// <summary>
        /// Marks this message as completed, but not found.
        /// </summary>
        public void NotFound()
        {
            status = Status.NotFound;
        }

        [Conditional("DEBUG")]
        private readonly void ThrowIfNotFound()
        {
            if (status != Status.Found)
            {
                throw new InvalidOperationException($"Data for `{address}` is not available");
            }
        }

        [Conditional("DEBUG")]
        private readonly void ThrowIfFound()
        {
            if (status == Status.Found)
            {
                throw new InvalidOperationException($"Data for `{address}` is already loaded");
            }
        }

        /// <summary>
        /// Possible states of the message.
        /// </summary>
        public enum Status : byte
        {
            /// <summary>
            /// The message hasn't been handled.
            /// </summary>
            NotHandled,

            /// <summary>
            /// Data has been found.
            /// </summary>
            Found,

            /// <summary>
            /// Data has not been found.
            /// </summary>
            NotFound,

            /// <summary>
            /// Data has been found and was consumed by a handler.
            /// </summary>
            FoundAndConsumed
        }
    }
}