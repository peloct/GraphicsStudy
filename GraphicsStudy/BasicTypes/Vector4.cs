using System;

namespace GraphicsStudy.BasicTypes
{
    public struct Vector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public float Magnitude { get { return (float)Math.Sqrt(x * x + y * y + z * z + w * w); } }

        public float SqrMagnitude { get { return x * x + y * y + z * z + w * w; } }

        public float this[int index]
        {
            get
            {
                if (index == 0)
                    return x;
                else if (index == 1)
                    return y;
                else if (index == 2)
                    return z;
                else if (index == 3)
                    return w;
                else
                    throw new IndexOutOfRangeException();
            }

            set
            {
                if (index == 0)
                    x = value;
                else if (index == 1)
                    y = value;
                else if (index == 2)
                    z = value;
                else if (index == 3)
                    w = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public Vector4(float x, float y, float z, float w) { this.x = x; this.y = y; this.z = z; this.w = w; }

        public static Vector4 operator +(Vector4 a) { return a; }

        public static Vector4 operator -(Vector4 a) { return new Vector4(-a.x, -a.y, -a.z, -a.w); }

        public static Vector4 operator +(Vector4 a, Vector4 b) { return new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w); }

        public static Vector4 operator -(Vector4 a, Vector4 b) { return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w); }

        public static Vector4 operator *(Vector4 a, float b) { return new Vector4(a.x * b, a.y * b, a.z * b, a.w * b); }

        public static Vector4 operator *(float a, Vector4 b) { return new Vector4(b.x * a, b.y * a, b.z * a, b.w * a); }

        public static Vector4 operator /(Vector4 a, float b) { return new Vector4(a.x / b, a.y / b, a.z / b, a.w / b); }

        public static float Dot(Vector4 a, Vector4 b) { return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w; }

        public void Normalize()
        {
            double length = Math.Sqrt(x * x + y * y + z * z + w * w);
            x = (float)(x / length);
            y = (float)(y / length);
            z = (float)(z / length);
            w = (float)(w / length);
        }

        public Vector4 Normalized()
        {
            double length = Math.Sqrt(x * x + y * y + z * z + w * w);
            return new Vector4((float)(x / length), (float)(y / length), (float)(z / length), (float)(w / length));
        }
    }
}
