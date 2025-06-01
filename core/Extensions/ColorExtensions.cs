using System.Numerics;

namespace Data
{
    /// <summary>
    /// Extensions for working with <see cref="Vector4"/> as if its a <see cref="Color"/>.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Retrieves the hue of the vector color.
        /// </summary>
        public static float GetHue(this Vector4 rgba)
        {
            return new Color(rgba).Hue;
        }
    }
}
