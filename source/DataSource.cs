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

        public readonly USpan<byte> Bytes => entity.GetArray<byte>();
        public readonly FixedString Address => entity.GetComponentRef<IsDataSource>().address;

        readonly uint IEntity.Value => entity.value;
        readonly World IEntity.World => entity.world;
        readonly Definition IEntity.Definition => new Definition().AddComponentType<IsDataSource>().AddArrayType<byte>();

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
        public DataSource(World world, USpan<char> address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray<byte>(0);
        }

        /// <summary>
        /// Creates an empty data source.
        /// </summary>
        public DataSource(World world, FixedString address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray<byte>(0);
        }

        /// <summary>
        /// Creates a data source containing the given bytes.
        /// </summary>
        public DataSource(World world, USpan<char> address, USpan<byte> bytes)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray(bytes);
        }

        /// <summary>
        /// Creates a data source containing the given bytes.
        /// </summary>
        public DataSource(World world, FixedString address, USpan<byte> bytes)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray(bytes);
        }

        /// <summary>
        /// Creates a data source containing the given text as UTF8 encoded bytes.
        /// </summary>
        public DataSource(World world, USpan<char> address, USpan<char> text)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray<byte>(0);
            Write(text);
        }

        /// <summary>
        /// Creates a data source containing the given text as UTF8 encoded bytes.
        /// </summary>
        public DataSource(World world, FixedString address, USpan<char> text)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray<byte>(0);
            Write(text);
        }

        /// <summary>
        /// Creates a data source containing the given text as UTF8 encoded bytes.
        /// </summary>
        public DataSource(World world, FixedString address, string text)
        {
            entity = new(world);
            entity.AddComponent(new IsDataSource(address));
            entity.CreateArray<byte>(0);
            Write(text);
        }

        public unsafe readonly override string ToString()
        {
            USpan<char> buffer = stackalloc char[(int)FixedString.MaxLength];
            uint length = ToString(buffer);
            return new string(buffer.pointer, 0, (int)length);
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            FixedString name = Address;
            return name.CopyTo(buffer);
        }

        public readonly void Clear()
        {
            entity.ResizeArray<byte>(0);
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public readonly void Write(USpan<char> text)
        {
            using BinaryWriter writer = BinaryWriter.Create();
            writer.WriteUTF8Text(text);
            Write(writer.GetBytes());
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public readonly void Write(FixedString text)
        {
            using BinaryWriter writer = BinaryWriter.Create();
            writer.WriteUTF8Text(text);
            Write(writer.GetBytes());
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public readonly void Write(string text)
        {
            using BinaryWriter writer = BinaryWriter.Create();
            writer.WriteUTF8Text(text);
            Write(writer.GetBytes());
        }

        /// <summary>
        /// Appends the given bytes.
        /// </summary>
        public readonly void Write(USpan<byte> bytes)
        {
            USpan<byte> array = entity.ResizeArray<byte>(bytes.length);
            bytes.CopyTo(array);
        }
    }
}
