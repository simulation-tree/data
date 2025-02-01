using Data.Components;
using System;
using Unmanaged;
using Worlds;

namespace Data
{
    /// <summary>
    /// Represents a span of <see cref="byte"/> that can be found with
    /// a <see cref="DataRequest"/>.
    /// </summary>
    public readonly struct DataSource : IDataSource
    {
        private readonly Entity entity;

        readonly uint IEntity.Value => entity.value;
        readonly World IEntity.World => entity.world;

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<IsDataSource>();
            archetype.AddArrayElementType<BinaryData>();
        }

#if NET
        [Obsolete("Default constructor not supported.", true)]
        public DataSource()
        {
            throw new NotSupportedException();
        }
#endif

        /// <summary>
        /// Creates an empty data source.
        /// </summary>
        public DataSource(World world, Address address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray<BinaryData>();
        }

        /// <summary>
        /// Creates a data source containing the given bytes.
        /// </summary>
        public DataSource(World world, Address address, USpan<byte> bytes)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray(bytes.As<BinaryData>());
        }

        /// <summary>
        /// Creates a data source containing the given text as UTF8 encoded bytes.
        /// </summary>
        public DataSource(World world, Address address, USpan<char> text)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray<BinaryData>();
            this.WriteUTF8(text);
        }

        /// <summary>
        /// Creates a data source containing the given text as UTF8 encoded bytes.
        /// </summary>
        public DataSource(World world, Address address, string text)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray<BinaryData>();
            this.WriteUTF8(text);
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        public readonly override string ToString()
        {
            USpan<char> buffer = stackalloc char[(int)FixedString.Capacity];
            uint length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            return this.GetSourceAddress().ToString(buffer);
        }
    }
}