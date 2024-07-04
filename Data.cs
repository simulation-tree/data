using Data.Components;
using Simulation;
using System;
using Unmanaged;
using Unmanaged.Collections;

namespace Data
{
    /// <summary>
    /// Represents a span of <see cref="byte"/> that can be found with
    /// a <see cref="DataRequest"/>.
    /// </summary>
    public readonly struct Data : IDisposable
    {
        private readonly Entity entity;
        private readonly UnmanagedList<byte> bytes;

        public readonly bool IsDestroyed => entity.IsDestroyed;
        public readonly ReadOnlySpan<byte> Bytes => bytes.AsSpan();
        public readonly FixedString Name => entity.GetComponent<IsData>().name;

        public Data()
        {
            throw new InvalidOperationException("Cannot create a resource without a world.");
        }

        public Data(World world, ReadOnlySpan<char> fileName)
        {
            entity = new(world);
            entity.AddComponent(new IsData(fileName));
            bytes = entity.CreateCollection<byte>();
        }

        public Data(World world, ReadOnlySpan<char> fileName, ReadOnlySpan<byte> bytes)
        {
            entity = new(world);
            entity.AddComponent(new IsData(fileName));
            this.bytes = entity.CreateCollection<byte>((uint)(bytes.Length + 1));
            this.bytes.AddRange(bytes);
        }

        public Data(World world, ReadOnlySpan<char> fileName, ReadOnlySpan<char> text)
        {
            entity = new(world);
            entity.AddComponent(new IsData(fileName));
            using BinaryWriter writer = new();
            writer.WriteUTF8Span(text);
            bytes = entity.CreateCollection<byte>(writer.Position + 1);
            bytes.AddRange(writer.AsSpan());
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        public readonly override string ToString()
        {
            FixedString name = Name;
            Span<char> buffer = stackalloc char[name.Length];
            name.CopyTo(buffer);
            return new string(buffer);
        }

        public readonly void Clear()
        {
            bytes.Clear();
        }

        public readonly void Write(ReadOnlySpan<char> text)
        {
            using BinaryWriter writer = new();
            writer.WriteUTF8Span(text);
            Write(writer.AsSpan());
        }

        public readonly void Write(ReadOnlySpan<byte> bytes)
        {
            this.bytes.AddRange(bytes);
        }
    }
}
