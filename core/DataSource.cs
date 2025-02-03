using Data.Components;
using Unmanaged;
using Worlds;

namespace Data
{
    /// <summary>
    /// Represents a span of <see cref="byte"/> that can be found with
    /// a <see cref="DataRequest"/>.
    /// </summary>
    public readonly partial struct DataSource : IEntity
    {
        public readonly Address Address => GetComponent<IsDataSource>().address;
        public readonly USpan<byte> Bytes => GetArray<BinaryData>().As<byte>();

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<IsDataSource>();
            archetype.AddArrayType<BinaryData>();
        }

        /// <summary>
        /// Creates an empty data source.
        /// </summary>
        public DataSource(World world, Address address)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataSource(address));
            world.CreateArray<BinaryData>(value);
        }

        /// <summary>
        /// Creates a data source containing the given bytes.
        /// </summary>
        public DataSource(World world, Address address, USpan<byte> bytes)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataSource(address));
            world.CreateArray(value, bytes.As<BinaryData>());
        }

        /// <summary>
        /// Creates a data source containing the given text as UTF8 encoded bytes.
        /// </summary>
        public DataSource(World world, Address address, USpan<char> text)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataSource(address));
            world.CreateArray<BinaryData>(value);
            WriteUTF8(text);
        }

        /// <summary>
        /// Creates a data source containing the given text as UTF8 encoded bytes.
        /// </summary>
        public DataSource(World world, Address address, string text)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataSource(address));
            world.CreateArray<BinaryData>(value);
            WriteUTF8(text);
        }

        public readonly override string ToString()
        {
            USpan<char> buffer = stackalloc char[(int)FixedString.Capacity];
            uint length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        public readonly uint ToString(USpan<char> buffer)
        {
            return Address.ToString(buffer);
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public readonly void WriteUTF8(USpan<char> text)
        {
            using BinaryWriter writer = new(4);
            writer.WriteUTF8(text);
            Write(writer.AsSpan());
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public readonly void WriteUTF8(FixedString text)
        {
            using BinaryWriter writer = new(4);
            writer.WriteUTF8(text);
            Write(writer.AsSpan());
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public readonly void WriteUTF8(string text)
        {
            using BinaryWriter writer = new(4);
            writer.WriteUTF8(text);
            Write(writer.AsSpan());
        }

        /// <summary>
        /// Appends the given bytes.
        /// </summary>
        public readonly void Write(USpan<byte> bytes)
        {
            uint length = GetArrayLength<BinaryData>();
            USpan<BinaryData> array = ResizeArray<BinaryData>(bytes.Length + length);
            bytes.CopyTo(array.As<byte>());
        }

        public readonly BinaryReader CreateBinaryReader()
        {
            return new(Bytes);
        }
    }
}