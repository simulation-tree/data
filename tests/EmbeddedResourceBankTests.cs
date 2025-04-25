using Data.Functions;
using System;
using Unmanaged;

namespace Data.Tests
{
    public class EmbeddedResourceBankTests : DataTests
    {
        [Test]
        public void LoadEmbeddedResource()
        {
            Assert.That(EmbeddedResourceRegistry.Contains("Assets/data1.txt"), Is.False);

            EmbeddedResourceRegistry.Load<CustomResourceBank>();

            Assert.That(EmbeddedResourceRegistry.Contains("Assets/data1.txt"), Is.True);
            EmbeddedResource embeddedResource = EmbeddedResourceRegistry.Get("Assets/data1.txt");
            Assert.That(embeddedResource.Assembly, Is.EqualTo(typeof(CustomResourceBank).Assembly));
            Assert.That(embeddedResource.address.ToString(), Is.EqualTo("Assets/data1.txt"));
            using ByteReader data = embeddedResource.CreateByteReader();
            Span<char> buffer = stackalloc char[128];
            int length = data.ReadUTF8(buffer);
            Assert.That(buffer.Slice(0, length).ToString(), Is.EqualTo("this is some data, yo"));
        }

        public readonly struct CustomResourceBank : IEmbeddedResourceBank
        {
            void IEmbeddedResourceBank.Load(Register register)
            {
                register.Invoke("Assets/data1.txt");
            }
        }
    }
}