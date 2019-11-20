using System;

namespace GraphicsStudy
{
    public struct Matrix2
    {
        public float m00;
        public float m10;
        public float m01;
        public float m11;

        public Vector2 this[int index]
        {
            get
            {
                if (index == 0)
                    return new Vector2(m00, m10);
                else if (index == 1)
                    return new Vector2(m01, m11);
                else
                    throw new IndexOutOfRangeException();
            }

            set
            {
                if (index == 0)
                {
                    m00 = value.x;
                    m10 = value.y;
                }
                else if (index == 1)
                {
                    m01 = value.x;
                    m11 = value.y;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public float this[int row, int col]
        {
            get
            {
                if (row == 0)
                {
                    if (col == 0)
                        return m00;
                    else if (col == 1)
                        return m01;
                    else
                        throw new IndexOutOfRangeException();
                }
                else if (row == 1)
                {
                    if (col == 0)
                        return m10;
                    else if (col == 1)
                        return m11;
                    else
                        throw new IndexOutOfRangeException();
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }

            set
            {
                if (row == 0)
                {
                    if (col == 0)
                        m00 = value;
                    else if (col == 1)
                        m01 = value;
                    else
                        throw new IndexOutOfRangeException();
                }
                else if (row == 1)
                {
                    if (col == 0)
                        m10 = value;
                    else if (col == 1)
                        m11 = value;
                    else
                        throw new IndexOutOfRangeException();
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public Matrix2(float m00, float m01, float m10, float m11)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m10 = m10;
            this.m11 = m11;
        }

        public Matrix2(Vector2 c0, Vector2 c1)
        {
            m00 = c0.x;
            m10 = c0.y;
            m01 = c1.x;
            m11 = c1.y;
        }

        public static Matrix2 operator +(Matrix2 a) { return a; }

        public static Matrix2 operator -(Matrix2 a) { return new Matrix2(-a.m00, -a.m01, -a.m10, -a.m11); }

        public static Matrix2 operator +(Matrix2 a, Matrix2 b) { return new Matrix2(a.m00 + b.m00, a.m01 + b.m01, a.m10 + b.m10, a.m11 + b.m11); }

        public static Matrix2 operator -(Matrix2 a, Matrix2 b) { return new Matrix2(a.m00 - b.m00, a.m01 - b.m01, a.m10 - b.m10, a.m11 - b.m11); }

        public static Matrix2 operator *(Matrix2 a, float b) { return new Matrix2(a.m00 * b, a.m01 * b, a.m10 * b, a.m11 * b); }

        public static Matrix2 operator *(float a, Matrix2 b) { return new Matrix2(b.m00 * a, b.m01 * a, b.m10 * a, b.m11 * a); }

        public static Matrix2 operator /(Matrix2 a, float b) { return new Matrix2(a.m00 / b, a.m01 / b, a.m10 / b, a.m11 / b); }

        public static Vector2 operator *(Matrix2 a, Vector2 b)
        {
            return new Vector2(
                a.m00 * b.x + a.m01 * b.y,
                a.m10 * b.x + a.m11 * b.y);
        }

        public static Matrix2 operator *(Matrix2 a, Matrix2 b)
        {
            return new Matrix2(
                a.m00 * b.m00 + a.m01 * b.m10,
                a.m00 * b.m01 + a.m01 * b.m11,
                a.m10 * b.m00 + a.m11 * b.m10,
                a.m10 * b.m01 + a.m11 * b.m11);
        }

        public Matrix2 Transpose()
        {
            return new Matrix2(
                m00, m10,
                m01, m11);
        }
    }
}
