using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Data
{
    /// <summary>
    /// RGBA color type.
    /// </summary>
    public struct Color : IEquatable<Color>
    {
        /// <summary>
        /// The gap between two colors to be considered equal in 32 bits.
        /// </summary>
        public const float Precision = 1 / 255f;

        /// <summary>
        /// White
        /// </summary>
        public static readonly Color White = new(1, 1, 1, 1);

        /// <summary>
        /// Black
        /// </summary>
        public static readonly Color Black = new(0, 0, 0, 1);

        /// <summary>
        /// Grey
        /// </summary>
        public static readonly Color Grey = new(0.5f, 0.5f, 0.5f, 1);

        /// <summary>
        /// Red
        /// </summary>
        public static readonly Color Red = new(1, 0, 0, 1);

        /// <summary>
        /// Green
        /// </summary>
        public static readonly Color Green = new(0, 1, 0, 1);

        /// <summary>
        /// Blue
        /// </summary>
        public static readonly Color Blue = new(0, 0, 1, 1);

        /// <summary>
        /// Yellow (Red + Green)
        /// </summary>
        public static readonly Color Yellow = new(1, 1, 0, 1);

        /// <summary>
        /// Cyan (Green + Blue)
        /// </summary>
        public static readonly Color Cyan = new(0, 1, 1, 1);

        /// <summary>
        /// Magenta (Red + Blue)
        /// </summary>
        public static readonly Color Magenta = new(1, 0, 1, 1);

        /// <summary>
        /// Orange (Mix of Red and Yellow)
        /// </summary>
        public static readonly Color Orange = new(1, 0.5f, 0, 1);

        /// <summary>
        /// Chartreuse (Mix of Green and Yellow)
        /// </summary>
        public static readonly Color Chartreuse = new(0.5f, 1, 0f, 1);

        /// <summary>
        /// Spring Green (Mix of Green and Cyan)
        /// </summary>
        public static readonly Color SpringGreen = new(0, 1, 0.5f, 1);

        /// <summary>
        /// Sky Blue (Mix of Blue and Cyan)
        /// </summary>
        public static readonly Color SkyBlue = new(0, 0.5f, 1, 1);

        /// <summary>
        /// Violet (Mix of Blue and Magenta)
        /// </summary>
        public static readonly Color Violet = new(0.5f, 0, 1, 1);

        /// <summary>
        /// Rose (Mix of Red and Magenta)
        /// </summary>
        public static readonly Color Rose = new(1, 0, 0.5f, 1);

        /// <summary>
        /// Red channel value.
        /// </summary>
        public float r;

        /// <summary>
        /// Green channel value.
        /// </summary>
        public float g;

        /// <summary>
        /// Blue channel value.
        /// </summary>
        public float b;

        /// <summary>
        /// Alpha channel value.
        /// </summary>
        public float a;

        /// <summary>
        /// The hue, saturation, value of the color.
        /// </summary>
        public Vector4 HSV
        {
            readonly get
            {
                float max = Math.Max(r, Math.Max(g, b));
                float min = Math.Min(r, Math.Min(g, b));
                float delta = max - min;
                float hue = 0f;
                if (delta != 0)
                {
                    if (max == r)
                    {
                        hue = (g - b) / delta;
                    }
                    else if (max == g)
                    {
                        hue = 2 + (b - r) / delta;
                    }
                    else
                    {
                        hue = 4 + (r - g) / delta;
                    }
                    hue *= 60;
                    if (hue < 0)
                    {
                        hue += 360;
                    }
                }

                hue /= 360;
                float saturation = max == 0 ? 0 : delta / max;
                float value = max;
                return new(hue, saturation, value, a);
            }
            set
            {
                this = FromHSV(value);
            }
        }

        /// <summary>
        /// The hue of the color, in the 0-1 range.
        /// </summary>
        public float Hue
        {
            readonly get
            {
                float max = Math.Max(r, Math.Max(g, b));
                float min = Math.Min(r, Math.Min(g, b));
                float delta = max - min;
                float hue = 0f;
                if (delta != 0)
                {
                    if (max == r)
                    {
                        hue = (g - b) / delta;
                    }
                    else if (max == g)
                    {
                        hue = 2 + (b - r) / delta;
                    }
                    else
                    {
                        hue = 4 + (r - g) / delta;
                    }

                    hue *= 60;
                    if (hue < 0)
                    {
                        hue += 360;
                    }
                }

                return hue / 360;
            }
            set
            {
                Vector4 hsv = HSV;
                hsv.X = value;
                this = FromHSV(hsv);
            }
        }

        /// <summary>
        /// The saturation value of the color, in the 0-1 range.
        /// </summary>
        public float Saturation
        {
            readonly get
            {
                float max = Math.Max(r, Math.Max(g, b));
                float min = Math.Min(r, Math.Min(r, b));
                float delta = max - min;
                return max == 0 ? 0 : delta / max;
            }
            set
            {
                Vector4 hsv = HSV;
                hsv.Y = value;
                this = FromHSV(hsv);
            }
        }

        /// <summary>
        /// The value of the color in the 0-1 range.
        /// </summary>
        public float Value
        {
            readonly get
            {
                return Math.Max(r, Math.Max(g, b));
            }
            set
            {
                Vector4 hsv = HSV;
                hsv.Z = value;
                this = FromHSV(hsv);
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Color(float value, float a = 1f)
        {
            r = value;
            g = value;
            b = value;
            this.a = a;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Color(Vector4 rgba)
        {
            r = rgba.X;
            g = rgba.Y;
            b = rgba.Z;
            a = rgba.W;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Color(Vector3 rgb, float a)
        {
            r = rgb.X;
            g = rgb.Y;
            b = rgb.Z;
            this.a = a;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Color(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Color(int rgba)
        {
            r = ((rgba >> 24) & 0xFF) * Precision;
            g = ((rgba >> 16) & 0xFF) * Precision;
            b = ((rgba >> 8) & 0xFF) * Precision;
            a = (rgba & 0xFF) * Precision;
        }

        /// <inheritdoc/>
        public readonly override string ToString()
        {
            Span<char> buffer = stackalloc char[256];
            int length = ToString(buffer);
            return buffer.Slice(0, length).ToString();
        }

        /// <inheritdoc/>
        [SkipLocalsInit]
        public readonly int ToString(Span<char> destination)
        {
            int length = 0;
            destination[length++] = 'R';
            destination[length++] = 'G';
            destination[length++] = 'B';
            destination[length++] = 'A';
            destination[length++] = '(';
            length += r.ToString(destination.Slice(length));
            destination[length++] = ' ';
            destination[length++] = ',';
            length += g.ToString(destination.Slice(length));
            destination[length++] = ' ';
            destination[length++] = ',';
            length += b.ToString(destination.Slice(length));
            destination[length++] = ' ';
            destination[length++] = ',';
            length += a.ToString(destination.Slice(length));
            destination[length++] = ')';
            return length;
        }

        /// <inheritdoc/>
        public readonly override bool Equals(object? obj)
        {
            return obj is Color color && Equals(color);
        }

        /// <inheritdoc/>
        public readonly bool Equals(Color other)
        {
            return r == other.r && g == other.g && b == other.b && a == other.a;
        }

        /// <summary>
        /// Does a comparison between two colors with a <paramref name="threshold"/>.
        /// </summary>
        public readonly bool Equals(Color other, float threshold = Precision)
        {
            return Math.Abs(r - other.r) < threshold && Math.Abs(g - other.g) < threshold && Math.Abs(b - other.b) < threshold && Math.Abs(a - other.a) < threshold;
        }

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + r.GetHashCode();
            hash = hash * 31 + g.GetHashCode();
            hash = hash * 31 + b.GetHashCode();
            hash = hash * 31 + a.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Retrieves this color as a <see cref="Vector4"/>.
        /// </summary>
        public readonly Vector4 AsVector4()
        {
            return new(r, g, b, a);
        }

        /// <summary>
        /// Creates a color value from byte values.
        /// </summary>
        public static Color CreateFromBytes(byte r, byte g, byte b, byte a = byte.MaxValue)
        {
            return new Color(r * Precision, g * Precision, b * Precision, a * Precision);
        }

        /// <summary>
        /// Creates a color from the given <paramref name="hsv"/>.
        /// </summary>
        public static Color FromHSV(Vector3 hsv, float a = 1)
        {
            float hue = hsv.X;
            float saturation = hsv.Y;
            float value = hsv.Z;
            ThrowIfOutOfRange(hue);

            byte pie = (byte)(hue * 6);
            float f = hue * 6 - pie;
            float p = value * (1 - saturation);
            float q = value * (1 - f * saturation);
            float t = value * (1 - (1 - f) * saturation);
            return pie switch
            {
                0 => new(value, t, p, a),
                1 => new(q, value, p, a),
                2 => new(p, value, t, a),
                3 => new(p, q, value, a),
                4 => new(t, p, value, a),
                5 => new(value, p, q, a),
                _ => default
            };
        }

        /// <summary>
        /// Creates a color from the given <paramref name="hsva"/>.
        /// </summary>
        public static Color FromHSV(Vector4 hsva)
        {
            float hue = hsva.X;
            float saturation = hsva.Y;
            float value = hsva.Z;
            float a = hsva.W;
            ThrowIfOutOfRange(hue);

            byte pie = (byte)(hue * 6);
            float f = hue * 6 - pie;
            float p = value * (1 - saturation);
            float q = value * (1 - f * saturation);
            float t = value * (1 - (1 - f) * saturation);
            return pie switch
            {
                0 => new(value, t, p, a),
                1 => new(q, value, p, a),
                2 => new(p, value, t, a),
                3 => new(p, q, value, a),
                4 => new(t, p, value, a),
                5 => new(value, p, q, a),
                _ => default
            };
        }

        /// <summary>
        /// Creates a color from the given hsva values.
        /// </summary>
        public static Color FromHSV(float h, float s, float v, float a)
        {
            ThrowIfOutOfRange(h);

            byte pie = (byte)(h * 6);
            float f = h * 6 - pie;
            float p = v * (1 - s);
            float q = v * (1 - f * s);
            float t = v * (1 - (1 - f) * s);
            return pie switch
            {
                0 => new(v, t, p, a),
                1 => new(q, v, p, a),
                2 => new(p, v, t, a),
                3 => new(p, q, v, a),
                4 => new(t, p, v, a),
                5 => new(v, p, q, a),
                _ => default
            };
        }

        [Conditional("DEBUG")]
        private static void ThrowIfOutOfRange(float hue)
        {
            if (hue < 0 || hue > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(hue), hue, "Hue must be between contained within the 0-1 range");
            }
        }

        /// <inheritdoc/>
        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(Color left, Color right)
        {
            return !(left == right);
        }
    }
}