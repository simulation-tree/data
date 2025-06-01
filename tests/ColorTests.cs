using System.Numerics;

namespace Data.Tests
{
    public class ColorTests
    {
        [Test]
        public void CheckColorConversion()
        {
            Color red = Color.FromHSV(0, 1, 1, 1);
            Assert.That(red, Is.EqualTo(new Color(1, 0, 0, 1)));

            Color green = Color.FromHSV(120f / 360f, 1, 1, 1);
            Assert.That(green, Is.EqualTo(new Color(0, 1, 0, 1)));

            Color blue = Color.FromHSV(240f / 360f, 1, 1, 1);
            Assert.That(blue, Is.EqualTo(new Color(0, 0, 1, 1)));

            Color white = Color.FromHSV(0, 0, 1, 1);
            Assert.That(white, Is.EqualTo(new Color(1, 1, 1, 1)));

            Vector4 doorhinge = new(0, 1f, 1f, 1f);
            Assert.That(doorhinge.GetHue(), Is.EqualTo(0.5f));
        }

        [Test]
        public void IncrementHue()
        {
            Color color = Color.Red;
            color.Hue += 0.5f;

            Assert.That(color, Is.EqualTo(Color.Cyan));
        }
    }
}