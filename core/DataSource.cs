﻿using Data.Components;
using System;
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
        /// <summary>
        /// Address defining the data source.
        /// </summary>
        public readonly ref Address Address => ref GetComponent<IsDataSource>().address;

        /// <summary>
        /// Assigned bytes.
        /// </summary>
        public readonly Span<byte> Bytes => GetArray<DataByte>().AsSpan<byte>();

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<IsDataSource>();
            archetype.AddArrayType<DataByte>();
        }

        /// <summary>
        /// Creates an empty data source.
        /// </summary>
        public DataSource(World world, Address address)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataSource(address));
            world.CreateArray<DataByte>(value);
        }

        /// <summary>
        /// Creates a data source containing the given bytes.
        /// </summary>
        public DataSource(World world, Address address, Span<byte> bytes)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataSource(address));
            world.CreateArray(value, bytes.As<byte, DataByte>());
        }

        /// <summary>
        /// Creates a data source containing the given text as UTF8 encoded bytes.
        /// </summary>
        public DataSource(World world, Address address, Span<char> text)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataSource(address));
            world.CreateArray<DataByte>(value);
            WriteUTF8(text);
        }

        /// <summary>
        /// Creates a data source containing the given text as UTF8 encoded bytes.
        /// </summary>
        public DataSource(World world, Address address, string text)
        {
            this.world = world;
            value = world.CreateEntity(new IsDataSource(address));
            world.CreateArray<DataByte>(value);
            WriteUTF8(text);
        }

        /// <inheritdoc/>
        public readonly override string ToString()
        {
            return Address.ToString();
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public readonly void WriteUTF8(ReadOnlySpan<char> text)
        {
            using ByteWriter writer = new(text.Length * 3);
            writer.WriteUTF8(text);
            Write(writer.AsSpan());
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public readonly void WriteUTF8(ASCIIText256 text)
        {
            using ByteWriter writer = new(text.Length * 3);
            writer.WriteUTF8(text);
            Write(writer.AsSpan());
        }

        /// <summary>
        /// Appends the given text as UTF8 formatted bytes.
        /// </summary>
        public readonly void WriteUTF8(string text)
        {
            using ByteWriter writer = new(text.Length * 3);
            writer.WriteUTF8(text);
            Write(writer.AsSpan());
        }

        /// <summary>
        /// Appends the given bytes.
        /// </summary>
        public readonly void Write(ReadOnlySpan<byte> bytes)
        {
            Values<DataByte> array = GetArray<DataByte>();
            array.AddRange(bytes.As<byte, DataByte>());
        }

        /// <summary>
        /// Clears the data source.
        /// </summary>
        public readonly void Clear()
        {
            Values<DataByte> array = GetArray<DataByte>();
            array.Clear();
        }

        /// <summary>
        /// Creates a new byte reader from the contained bytes.
        /// </summary>
        public readonly ByteReader CreateByteReader()
        {
            return new(Bytes);
        }
    }
}