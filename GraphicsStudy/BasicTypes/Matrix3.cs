using System;

namespace GraphicsStudy.BasicTypes
{
    public struct Matrix3
    {
        public float m00;
        public float m10;
        public float m20;
        public float m01;
        public float m11;
        public float m21;
        public float m02;
        public float m12;
        public float m22;

        public Vector3 this[int index]
        {
            get
            {
                if (index == 0)
                    return new Vector3(m00, m10, m20);
                else if (index == 1)
                    return new Vector3(m01, m11, m21);
                else if (index == 2)
                    return new Vector3(m02, m12, m22);
                else
                    throw new IndexOutOfRangeException();
            }

            set
            {
                if (index == 0)
                {
                    m00 = value.x;
                    m10 = value.y;
                    m20 = value.z;
                }
                else if (index == 1)
                {
                    m01 = value.x;
                    m11 = value.y;
                    m21 = value.z;
                }
                else if (index == 2)
                {
                    m02 = value.x;
                    m12 = value.y;
                    m22 = value.z;
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
                    else if (col == 2)
                        return m02;
                    else
                        throw new IndexOutOfRangeException();
                }
                else if (row == 1)
                {
                    if (col == 0)
                        return m10;
                    else if (col == 1)
                        return m11;
                    else if (col == 2)
                        return m12;
                    else
                        throw new IndexOutOfRangeException();
                }
                else if (row == 2)
                {
                    if (col == 0)
                        return m20;
                    else if (col == 1)
                        return m21;
                    else if (col == 2)
                        return m22;
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
                    else if (col == 2)
                        m02 = value;
                    else
                        throw new IndexOutOfRangeException();
                }
                else if (row == 1)
                {
                    if (col == 0)
                        m10 = value;
                    else if (col == 1)
                        m11 = value;
                    else if (col == 2)
                        m12 = value;
                    else
                        throw new IndexOutOfRangeException();
                }
                else if (row == 2)
                {
                    if (col == 0)
                        m20 = value;
                    else if (col == 1)
                        m21 = value;
                    else if (col == 2)
                        m22 = value;
                    else
                        throw new IndexOutOfRangeException();
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public Matrix3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
        {
            this.m00 = m00;
            this.m10 = m10;
            this.m20 = m20;
            this.m01 = m01;
            this.m11 = m11;
            this.m21 = m21;
            this.m02 = m02;
            this.m12 = m12;
            this.m22 = m22;
        }

        public Matrix3(Vector3 c0, Vector3 c1, Vector3 c2)
        {
            m00 = c0.x;
            m10 = c0.y;
            m20 = c0.z;
            m01 = c1.x;
            m11 = c1.y;
            m21 = c1.z;
            m02 = c2.x;
            m12 = c2.y;
            m22 = c2.z;
        }

        public static Matrix3 operator +(Matrix3 a) { return a; }

        public static Matrix3 operator -(Matrix3 a)
        {
            return new Matrix3(
                -a.m00, -a.m01, -a.m02,
                -a.m10, -a.m11, -a.m12,
                -a.m20, -a.m21, -a.m22);
        }

        public static Matrix3 operator +(Matrix3 a, Matrix3 b)
        {
            return new Matrix3(
                a.m00 + b.m00, a.m01 + b.m01, a.m02 + b.m02,
                a.m10 + b.m10, a.m11 + b.m11, a.m12 + b.m12,
                a.m20 + b.m20, a.m21 + b.m21, a.m22 + b.m22);
        }

        public static Matrix3 operator -(Matrix3 a, Matrix3 b)
        {
            return new Matrix3(
                a.m00 - b.m00, a.m01 - b.m01, a.m02 - b.m02,
                a.m10 - b.m10, a.m11 - b.m11, a.m12 - b.m12,
                a.m20 - b.m20, a.m21 - b.m21, a.m22 - b.m22);
        }

        public static Matrix3 operator *(Matrix3 a, float b)
        {
            return new Matrix3(
                a.m00 * b, a.m01 * b, a.m02 * b,
                a.m10 * b, a.m11 * b, a.m12 * b,
                a.m20 * b, a.m21 * b, a.m22 * b);
        }

        public static Matrix3 operator *(float a, Matrix3 b)
        {
            return new Matrix3(
                b.m00 * a, b.m01 * a, b.m02 * a,
                b.m10 * a, b.m11 * a, b.m12 * a,
                b.m20 * a, b.m21 * a, b.m22 * a);
        }

        public static Matrix3 operator /(Matrix3 a, float b)
        {
            return new Matrix3(
                a.m00 / b, a.m01 / b, a.m02 / b,
                a.m10 / b, a.m11 / b, a.m12 / b,
                a.m20 / b, a.m21 / b, a.m22 / b);
        }

        public static Vector3 operator *(Matrix3 a, Vector3 b)
        {
            return new Vector3(
                a.m00 * b.x + a.m01 * b.y + a.m02 * b.z,
                a.m10 * b.x + a.m11 * b.y + a.m12 * b.z,
                a.m20 * b.x + a.m21 * b.y + a.m22 * b.z);
        }

        public static Matrix3 operator *(Matrix3 a, Matrix3 b)
        {
            return new Matrix3(
                a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20,
                a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21,
                a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22,
                a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20,
                a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21,
                a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22,
                a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20,
                a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21,
                a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22);
        }

        public Matrix3 Transpose()
        {
            return new Matrix3(
                m00, m10, m20,
                m01, m11, m21,
                m02, m12, m22);
        }
    }
}
