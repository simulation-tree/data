using Data.Components;
using Types;
using Unmanaged;
using Unmanaged.Tests;
using Worlds;

namespace Data.Tests
{
    public class DataTests : UnmanagedTests
    {
        static DataTests()
        {
            TypeLayout.Register<IsDataRequest>();
            TypeLayout.Register<IsDataSource>();
            TypeLayout.Register<IsData>();
            TypeLayout.Register<BinaryData>();
        }

        [Test]
        public void LoadDataFromEntity()
        {
            using World world = CreateWorld();
            DataSource data = new(world, "hello", "data");

            Assert.That(data.Address.ToString(), Is.EqualTo("hello"));

            using BinaryReader reader = new(data.Bytes);
            USpan<char> buffer = stackalloc char[128];
            uint length = reader.ReadUTF8Span(buffer);

            Assert.That(buffer.Slice(0, length).ToString(), Is.EqualTo("data"));
        }

        private static World CreateWorld()
        {
            Schema schema = new();
            schema.RegisterComponent<IsDataRequest>();
            schema.RegisterComponent<IsDataSource>();
            schema.RegisterComponent<IsData>();
            schema.RegisterArrayElement<BinaryData>();
            return new(schema);
        }
    }
}