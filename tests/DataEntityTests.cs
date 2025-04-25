using System;
using Unmanaged;
using Worlds;

namespace Data.Tests
{
    public class DataEntityTests : DataTests
    {
        [Test]
        public void LoadDataFromEntity()
        {
            using World world = CreateWorld();
            DataSource data = new(world, "hello", "data");

            Assert.That(data.Address.ToString(), Is.EqualTo("hello"));

            using ByteReader reader = data.CreateByteReader();
            Span<char> buffer = stackalloc char[128];
            int length = reader.ReadUTF8(buffer);

            Assert.That(buffer.Slice(0, length).ToString(), Is.EqualTo("data"));
        }
    }
}