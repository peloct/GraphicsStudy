using System;

namespace GraphicsStudy.BasicTypes
{
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public float Magnitude { get { return (float)Math.Sqrt(x * x + y * y + z * z); } }

        public float SqrMagnitude { get { return x * x + y * y + z * z; } }

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
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }

        public static Vector3 operator +(Vector3 a) { return a; }

        public static Vector3 operator -(Vector3 a) { return new Vector3(-a.x, -a.y, -a.z); }

        public static Vector3 operator +(Vector3 a, Vector3 b) { return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z); }

        public static Vector3 operator -(Vector3 a, Vector3 b) { return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z); }

        public static Vector3 operator *(Vector3 a, float b) { return new Vector3(a.x * b, a.y * b, a.z * b); }

        public static Vector3 operator *(float a, Vector3 b) { return new Vector3(b.x * a, b.y * a, b.z * a); }

        public static Vector3 operator /(Vector3 a, float b) { return new Vector3(a.x / b, a.y / b, a.z / b); }

        public static float Dot(Vector3 a, Vector3 b) { return a.x * b.x + a.y * b.y + a.z * b.z; }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(a.y * b.x - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        public void Normalize()
        {
            double length = Math.Sqrt(x * x + y * y + z * z);
            x = (float)(x / length);
            y = (float)(y / length);
            z = (float)(z / length);
        }

        public Vector3 Normalized()
        {
            double length = Math.Sqrt(x * x + y * y + z * z);
            return new Vector3((float)(x / length), (float)(y / length), (float)(z / length));
        }
    }
}
