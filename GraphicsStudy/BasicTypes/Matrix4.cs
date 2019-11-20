using System;

namespace GraphicsStudy
{
    public struct Matrix4
    {
        public float m00;
        public float m10;
        public float m20;
        public float m30;
        public float m01;
        public float m11;
        public float m21;
        public float m31;
        public float m02;
        public float m12;
        public float m22;
        public float m32;
        public float m03;
        public float m13;
        public float m23;
        public float m33;

        public Vector4 this[int index]
        {
            get
            {
                if (index == 0)
                    return new Vector4(m00, m10, m20, m30);
                else if (index == 1)
                    return new Vector4(m01, m11, m21, m31);
                else if (index == 2)
                    return new Vector4(m02, m12, m22, m32);
                else if (index == 3)
                    return new Vector4(m03, m13, m23, m33);
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
                    m30 = value.w;
                }
                else if (index == 1)
                {
                    m01 = value.x;
                    m11 = value.y;
                    m21 = value.z;
                    m31 = value.w;
                }
                else if (index == 2)
                {
                    m02 = value.x;
                    m12 = value.y;
                    m22 = value.z;
                    m32 = value.w;
                }
                else if (index == 3)
                {
                    m03 = value.x;
                    m13 = value.y;
                    m23 = value.z;
                    m33 = value.w;
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
                    else if (col == 3)
                        return m03;
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
                    else if (col == 3)
                        return m13;
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
                    else if (col == 3)
                        return m23;
                    else
                        throw new IndexOutOfRangeException();
                }
                else if (row == 3)
                {
                    if (col == 0)
                        return m30;
                    else if (col == 1)
                        return m31;
                    else if (col == 2)
                        return m32;
                    else if (col == 3)
                        return m33;
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
                    else if (col == 3)
                        m03 = value;
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
                    else if (col == 3)
                        m13 = value;
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
                    else if (col == 3)
                        m23 = value;
                    else
                        throw new IndexOutOfRangeException();
                }
                else if (row == 3)
                {
                    if (col == 0)
                        m30 = value;
                    else if (col == 1)
                        m31 = value;
                    else if (col == 2)
                        m32 = value;
                    else if (col == 3)
                        m33 = value;
                    else
                        throw new IndexOutOfRangeException();
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public Matrix4(
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33)
        {
            this.m00 = m00;
            this.m10 = m10;
            this.m20 = m20;
            this.m30 = m30;
            this.m01 = m01;
            this.m11 = m11;
            this.m21 = m21;
            this.m31 = m31;
            this.m02 = m02;
            this.m12 = m12;
            this.m22 = m22;
            this.m32 = m32;
            this.m03 = m03;
            this.m13 = m13;
            this.m23 = m23;
            this.m33 = m33;
        }

        public Matrix4(Vector4 c0, Vector4 c1, Vector4 c2, Vector4 c3)
        {
            m00 = c0.x;
            m10 = c0.y;
            m20 = c0.z;
            m30 = c0.w;
            m01 = c1.x;
            m11 = c1.y;
            m21 = c1.z;
            m31 = c1.w;
            m02 = c2.x;
            m12 = c2.y;
            m22 = c2.z;
            m32 = c2.w;
            m03 = c3.x;
            m13 = c3.y;
            m23 = c3.z;
            m33 = c3.w;
        }

        public static Matrix4 operator +(Matrix4 a) { return a; }

        public static Matrix4 operator -(Matrix4 a)
        {
            return new Matrix4(
                -a.m00, -a.m01, -a.m02, -a.m03,
                -a.m10, -a.m11, -a.m12, -a.m13,
                -a.m20, -a.m21, -a.m22, -a.m23,
                -a.m30, -a.m31, -a.m32, -a.m33);
        }

        public static Matrix4 operator +(Matrix4 a, Matrix4 b)
        {
            return new Matrix4(
                a.m00 + b.m00, a.m01 + b.m01, a.m02 + b.m02, a.m03 + b.m03,
                a.m10 + b.m10, a.m11 + b.m11, a.m12 + b.m12, a.m13 + b.m13,
                a.m20 + b.m20, a.m21 + b.m21, a.m22 + b.m22, a.m23 + b.m23,
                a.m30 + b.m30, a.m31 + b.m31, a.m32 + b.m32, a.m33 + b.m33);
        }

        public static Matrix4 operator -(Matrix4 a, Matrix4 b)
        {
            return new Matrix4(
                a.m00 - b.m00, a.m01 - b.m01, a.m02 - b.m02, a.m03 - b.m03,
                a.m10 - b.m10, a.m11 - b.m11, a.m12 - b.m12, a.m13 - b.m13,
                a.m20 - b.m20, a.m21 - b.m21, a.m22 - b.m22, a.m23 - b.m23,
                a.m30 - b.m30, a.m31 - b.m31, a.m32 - b.m32, a.m33 - b.m33);
        }

        public static Matrix4 operator *(Matrix4 a, float b)
        {
            return new Matrix4(
                 a.m00 * b, a.m01 * b, a.m02 * b, a.m03 * b,
                 a.m10 * b, a.m11 * b, a.m12 * b, a.m13 * b,
                 a.m20 * b, a.m21 * b, a.m22 * b, a.m23 * b,
                 a.m30 * b, a.m31 * b, a.m32 * b, a.m33 * b);
        }

        public static Matrix4 operator *(float a, Matrix4 b)
        {
            return new Matrix4(
                    b.m00 * a, b.m01 * a, b.m02 * a, b.m03 * a,
                    b.m10 * a, b.m11 * a, b.m12 * a, b.m13 * a,
                    b.m20 * a, b.m21 * a, b.m22 * a, b.m23 * a,
                    b.m30 * a, b.m31 * a, b.m32 * a, b.m33 * a);
        }

        public static Matrix4 operator /(Matrix4 a, float b)
        {
            return new Matrix4(
                  a.m00 / b, a.m01 / b, a.m02 / b, a.m03 / b,
                  a.m10 / b, a.m11 / b, a.m12 / b, a.m13 / b,
                  a.m20 / b, a.m21 / b, a.m22 / b, a.m23 / b,
                  a.m30 / b, a.m31 / b, a.m32 / b, a.m33 / b);
        }

        public static Vector4 operator *(Matrix4 a, Vector4 b)
        {
            return new Vector4(
                a.m00 * b.x + a.m01 * b.y + a.m02 * b.z + a.m03 * b.w,
                a.m10 * b.x + a.m11 * b.y + a.m12 * b.z + a.m13 * b.w,
                a.m20 * b.x + a.m21 * b.y + a.m22 * b.z + a.m23 * b.w,
                a.m30 * b.x + a.m31 * b.y + a.m32 * b.z + a.m33 * b.w);
        }

        public static Matrix4 operator *(Matrix4 a, Matrix4 b)
        {
            return new Matrix4(
                a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20 + a.m03 * b.m30,
                a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21 + a.m03 * b.m31,
                a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22 + a.m03 * b.m32,
                a.m00 * b.m03 + a.m01 * b.m13 + a.m02 * b.m23 + a.m03 * b.m33,
                a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20 + a.m13 * b.m30,
                a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31,
                a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32,
                a.m10 * b.m03 + a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33,
                a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20 + a.m23 * b.m30,
                a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31,
                a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32,
                a.m20 * b.m03 + a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33,
                a.m30 * b.m00 + a.m31 * b.m10 + a.m32 * b.m20 + a.m33 * b.m30,
                a.m30 * b.m01 + a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31,
                a.m30 * b.m02 + a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32,
                a.m30 * b.m03 + a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33);
        }

        public Matrix4 Transpose()
        {
            return new Matrix4(
                m00, m10, m20, m30,
                m01, m11, m21, m31,
                m02, m12, m22, m32,
                m03, m13, m23, m33);
        }
    }
}
