using Data.Components;
using Simulation;
using System;
using Unmanaged;

namespace Data
{
    /// <summary>
    /// Represents a span of <see cref="byte"/> that can be found with
    /// a <see cref="DataRequest"/>.
    /// </summary>
    public readonly struct DataSource : IDataSource
    {
        private readonly Entity entity;

        public readonly Span<byte> Bytes => entity.GetArray<byte>();
        public readonly FixedString Address => entity.GetComponent<IsDataSource>().address;

        World IEntity.World => entity;
        uint IEntity.Value => entity;

#if NET5_0_OR_GREATER
        [Obsolete("Default constructor not supported.", true)]
        public DataSource()
        {
            throw new NotSupportedException();
        }
#endif

        public DataSource(World world, ReadOnlySpan<char> address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray<byte>(0);
        }

        public DataSource(World world, ReadOnlySpan<char> address, ReadOnlySpan<byte> bytes)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray(bytes);
        }

        public DataSource(World world, ReadOnlySpan<char> address, ReadOnlySpan<char> text)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray<byte>(0);
            Write(text);
        }

        public readonly override string ToString()
        {
            FixedString name = this.Address;
            Span<char> buffer = stackalloc char[name.Length];
            name.ToString(buffer);
            return new string(buffer);
        }

        readonly Query IEntity.GetQuery(World world)
        {
            return new Query(world, RuntimeType.Get<IsDataSource>());
        }

        public readonly void Clear()
        {
            entity.ResizeArray<byte>(0);
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public readonly void Write(ReadOnlySpan<char> text)
        {
            using BinaryWriter writer = BinaryWriter.Create();
            writer.WriteUTF8Span(text);
            Write(writer.AsSpan());
        }

        /// <summary>
        /// Appends the given bytes.
        /// </summary>
        public readonly void Write(ReadOnlySpan<byte> bytes) 
        {
            Span<byte> array = entity.ResizeArray<byte>((uint)bytes.Length);
            bytes.CopyTo(array);
        }
    }
}
