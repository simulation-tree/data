using System;
using System.Numerics;

namespace Data
{
    public struct Color
    {
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
            readonly get
            {
                return Math.Max(value.X, Math.Max(value.Y, value.Z));
            }
            set
            {
                this = FromHSV(Hue, Saturation, value, Alpha);
            }
        }

        public float Alpha
        {
            readonly get
            {
                return value.W;
            }
            set
            {
                this.value = new(this.value.X, this.value.Y, this.value.Z, value);
            }
        }

        public float Red
        {
            readonly get
            {
                return value.X;
            }
            set
            {
                this.value = new(value, this.value.Y, this.value.Z, this.value.W);
            }
        }

        public float Green
        {
            readonly get
            {
                return value.Y;
            }
            set
            {
                this.value = new(this.value.X, value, this.value.Z, this.value.W);
            }
        }

        public float Blue
        {
            readonly get
            {
                return value.Z;
            }
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
            byte pie = (byte)(hue * 6);
            float f = hue * 6 - pie;
            float p = value * (1 - saturation);
            float q = value * (1 - f * saturation);
            float t = value * (1 - (1 - f) * saturation);
            switch (pie)
            {
                case 0:
                    return new Color(value, t, p, alpha);
                case 1:
                    return new Color(q, value, p, alpha);
                case 2:
                    return new Color(p, value, t, alpha);
                case 3:
                    return new Color(p, q, value, alpha);
                case 4:
                    return new Color(t, p, value, alpha);
                default:
                    return new Color(value, p, q, alpha);
            }
        }
    }
}
