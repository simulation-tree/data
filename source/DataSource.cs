using Data.Components;
using Simulation;
using System;
using Unmanaged;

namespace Data
{
    /// <summary>
    /// Represents a span of <see cref="byte"/> that can be found with
    /// a <see cref="DataEntity"/>.
    /// </summary>
    public readonly struct DataSource : IDataSource, IDisposable
    {
        private readonly Entity entity;

        public readonly Span<byte> Bytes => entity.GetList<byte>().AsSpan();
        public readonly FixedString Address => entity.GetComponent<IsDataSource>().address;

        World IEntity.World => entity;
        eint IEntity.Value => entity;

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
            entity.CreateList<byte>();
        }

        public DataSource(World world, ReadOnlySpan<char> address, ReadOnlySpan<byte> bytes)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateList<byte>();

            Write(bytes);
        }

        public DataSource(World world, ReadOnlySpan<char> address, ReadOnlySpan<char> text)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateList<byte>();

            Write(text);
        }

        public readonly void Dispose()
        {
            entity.Dispose();
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
            entity.GetList<byte>().Clear();
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
            entity.GetList<byte>().AddRange(bytes);
        }
    }
}
