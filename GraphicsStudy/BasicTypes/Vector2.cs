using System;

namespace GraphicsStudy
{
    public struct Vector2
    {
        public float x;
        public float y;

        public float Magnitude { get { return (float)Math.Sqrt(x * x + y * y); } }

        public float SqrMagnitude { get { return x * x + y * y; } }

        public float this[int index]
        {
            get
            {
                if (index == 0)
                    return x;
                else if (index == 1)
                    return y;
                else
                    throw new IndexOutOfRangeException();
            }

            set
            {
                if (index == 0)
                    x = value;
                else if (index == 1)
                    y = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public Vector2(float x, float y) { this.x = x; this.y = y; }

        public static Vector2 operator +(Vector2 a) { return a; }

        public static Vector2 operator -(Vector2 a) { return new Vector2(-a.x, -a.y); }

        public static Vector2 operator +(Vector2 a, Vector2 b) { return new Vector2(a.x + b.x, a.y + b.y); }

        public static Vector2 operator -(Vector2 a, Vector2 b) { return new Vector2(a.x - b.x, a.y - b.y); }

        public static Vector2 operator *(Vector2 a, float b) { return new Vector2(a.x * b, a.y * b); }

        public static Vector2 operator *(float a, Vector2 b) { return new Vector2(b.x * a, b.y * a); }

        public static Vector2 operator /(Vector2 a, float b) { return new Vector2(a.x / b, a.y / b); }

        public static float Dot(Vector2 a, Vector2 b) { return a.x * b.x + a.y * b.y; }

        public void Normalize()
        {
            double length = Math.Sqrt(x * x + y * y);
            x = (float)(x / length);
            y = (float)(y / length);
        }

        public Vector2 Normalized()
        {
            double length = Math.Sqrt(x * x + y * y);
            return new Vector2((float)(x / length), (float)(y / length));
        }
    }
}
