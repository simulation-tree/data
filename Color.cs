using System;
using System.Diagnostics;
using System.Numerics;

namespace Data
{
    public struct Color
    {
        public static readonly Color Black = new(0, 0, 0, 1);
        public static readonly Color White = new(1, 1, 1, 1);
        public static readonly Color Grey = new(0.5f, 0.5f, 0.5f, 1);

        public static readonly Color Red = new(1, 0, 0, 1);
        public static readonly Color Green = new(0, 1, 0, 1);
        public static readonly Color DeepBlue = new(0, 0, 1, 1);

        public static readonly Color Yellow = new(1, 1, 0, 1);
        public static readonly Color Cyan = new(0, 1, 1, 1);
        public static readonly Color Magenta = new(1, 0, 1, 1);

        public static readonly Color Orange = new(1, 0.5f, 0, 1);
        public static readonly Color Chartreuse = new(0, 1, 0.5f, 1);
        public static readonly Color SpringGreen = new(0, 0.5f, 1, 1);

        public static readonly Color Violet = new(0.5f, 0, 1, 1);
        public static readonly Color Rose = new(1, 0, 0.5f, 1);
        public static readonly Color SkyBlue = new(0, 0.5f, 1, 1);

        private Vector4 value;

        public float Hue
        {
            readonly get
            {
                float r = value.X;
                float g = value.Y;
                float b = value.Z;
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
                this = FromHSV(value, Saturation, Value, Alpha);
            }
        }

        public float Saturation
        {
            readonly get
            {
                float max = Math.Max(value.X, Math.Max(value.Y, value.Z));
                float min = Math.Min(value.X, Math.Min(value.Y, value.Z));
                float delta = max - min;
                return max == 0 ? 0 : delta / max;
            }
            set
            {
                this = FromHSV(Hue, value, Value, Alpha);
            }
        }

        public float Value
        {
            readonly get => Math.Max(value.X, Math.Max(value.Y, value.Z));
            set
            {
                this = FromHSV(Hue, Saturation, value, Alpha);
            }
        }

        public float Alpha
        {
            readonly get => value.W;
            set
            {
                this.value = new(this.value.X, this.value.Y, this.value.Z, value);
            }
        }

        public float RedChannel
        {
            readonly get => value.X;
            set
            {
                this.value = new(value, this.value.Y, this.value.Z, this.value.W);
            }
        }

        public float GreenChannel
        {
            readonly get => value.Y;
            set
            {
                this.value = new(this.value.X, value, this.value.Z, this.value.W);
            }
        }

        public float BlueChannel
        {
            readonly get => value.Z;
            set
            {
                this.value = new(this.value.X, this.value.Y, value, this.value.W);
            }
        }

        public Color(Vector4 value)
        {
            this.value = value;
        }

        public Color(float red, float green, float blue, float alpha = 1f)
        {
            value = new Vector4(red, green, blue, alpha);
        }

        public static Color FromHSV(float hue, float saturation, float value, float alpha = 1f)
        {
            ThrowIfOutOfRange(hue);
            byte pie = (byte)(hue * 6);
            float f = hue * 6 - pie;
            float p = value * (1 - saturation);
            float q = value * (1 - f * saturation);
            float t = value * (1 - (1 - f) * saturation);
            return pie switch
            {
                0 => new Color(value, t, p, alpha),
                1 => new Color(q, value, p, alpha),
                2 => new Color(p, value, t, alpha),
                3 => new Color(p, q, value, alpha),
                4 => new Color(t, p, value, alpha),
                5 => new Color(value, p, q, alpha),
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
    }
}
