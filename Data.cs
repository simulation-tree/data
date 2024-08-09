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
    public readonly struct Data : IData, IDisposable
    {
        private readonly Entity entity;

        World IEntity.World => entity.world;
        eint IEntity.Value => entity.value;

#if NET5_0_OR_GREATER
        [Obsolete("Default constructor not supported.", true)]
        public Data()
        {
            throw new NotSupportedException();
        }
#endif

        public Data(World world, ReadOnlySpan<char> address)
        {
            entity = new(world);
            entity.AddComponent(new IsData(address));
            entity.CreateList<Entity, byte>();
        }

        public Data(World world, ReadOnlySpan<char> address, ReadOnlySpan<byte> bytes)
        {
            entity = new(world);
            entity.AddComponent(new IsData(address));
            entity.CreateList<Entity, byte>();

            this.Write(bytes);
        }

        public Data(World world, ReadOnlySpan<char> address, ReadOnlySpan<char> text)
        {
            entity = new(world);
            entity.AddComponent(new IsData(address));
            entity.CreateList<Entity, byte>();

            this.Write(text);
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        public readonly override string ToString()
        {
            FixedString name = this.GetAddress();
            Span<char> buffer = stackalloc char[name.Length];
            name.CopyTo(buffer);
            return new string(buffer);
        }

        public static Query GetQuery(World world)
        {
            return new Query(world, RuntimeType.Get<IsData>());
        }
    }
}
