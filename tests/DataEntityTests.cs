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

            using BinaryReader reader = new(data.Bytes);
            USpan<char> buffer = stackalloc char[128];
            uint length = reader.ReadUTF8Span(buffer);

            Assert.That(buffer.Slice(0, length).ToString(), Is.EqualTo("data"));
        }
    }
}