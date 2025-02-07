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
        public readonly Entity entity;
        public readonly FixedString address;

        private readonly Array<byte> data;

        /// <summary>
        /// Checks if this message was completed.
        /// <para>
        /// If so, it should also be disposed.
        /// </para>
        /// </summary>
        public readonly bool IsLoaded => !data.IsDisposed;

        /// <summary>
        /// Loaded bytes.
        /// </summary>
        public readonly USpan<byte> Bytes
        {
            get
            {
                ThrowIfNotLoaded();

                return data.AsSpan();
            }
        }

        public LoadData(Entity entity, FixedString address)
        {
            this.entity = entity;
            this.address = address;
            this.data = default;
        }

        public LoadData(Entity entity, FixedString address, USpan<byte> data)
        {
            this.entity = entity;
            this.address = address;
            this.data = new(data);
        }

        public readonly LoadData BecomeLoaded(USpan<byte> data)
        {
            return new LoadData(entity, address, data);
        }

        public readonly void Dispose()
        {
            ThrowIfNotLoaded();

            data.Dispose();
        }

        [Conditional("DEBUG")]
        private readonly void ThrowIfNotLoaded()
        {
            if (!IsLoaded)
            {
                throw new InvalidOperationException($"Data for `{address}` is not available");
            }
        }
    }
}