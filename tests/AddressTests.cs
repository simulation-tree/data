namespace Data.Tests
{
    public class AddressTests
    {
        [Test]
        public void AddressEquality()
        {
            Address a = new("abacus");
            Assert.That(a.Matches("*/abacus"), Is.True);
        }
    }
}