using System;
using SkiaSharp;

namespace SkiaSharpFormsDemos.Transforms
{
    struct Matrix3D
    {
        float m11, m22, m33, m44;

        public float M11
        {
            set { m11 = value - 1; }
            get { return m11 + 1; }
        }
        public float M12 { set; get; }
        public float M13 { set; get; }
        public float M14 { set; get; }
        public float M21 { set; get; }
        public float M22
        {
            set { m22 = value - 1; }
            get { return m22 + 1; }
        }
        public float M23 { set; get; }
        public float M24 { set; get; }
        public float M31 { set; get; }
        public float M32 { set; get; }
        public float M33
        {
            set { m33 = value - 1; }
            get { return m33 + 1; }
        }
        public float M34 { set; get; }
        public float M41 { set; get; }
        public float M42 { set; get; }
        public float M43 { set; get; }
        public float M44
        {
            set { m44 = value - 1; }
            get { return m44 + 1; }
        }

        public SKMatrix Matrix2D
        {
            get
            {
                return new SKMatrix
                {
                    ScaleX = M11,
                    SkewX = M21,
                    TransX = M41,

                    SkewY = M12,
                    ScaleY = M22,
                    TransY = M42,

                    Persp0 = M14,
                    Persp1 = M24,
                    Persp2 = M44
                };
            }
        }

        public static Matrix3D RotationX(double degrees)
        {
            float sin, cos;
            GetSinCos(degrees, out sin, out cos);

            return new Matrix3D
            {
                M22 = cos,
                M23 = sin,
                M32 = -sin,
                M33 = cos
            };
        }

        public static Matrix3D RotationY(double degrees)
        {
            float sin, cos;
            GetSinCos(degrees, out sin, out cos);

            return new Matrix3D
            {
                M11 = cos,
                M13 = -sin,
                M31 = sin,
                M33 = cos
            };
        }

        public static Matrix3D RotationZ(double degrees)
        {
            float sin, cos;
            GetSinCos(degrees, out sin, out cos);

            return new Matrix3D
            {
                M11 = cos,
                M12 = sin,
                M21 = -sin,
                M22 = cos
            };
        }

        static void GetSinCos(double degrees, out float sin, out float cos)
        {
            double radians = Math.PI * degrees / 180;
            sin = (float)Math.Sin(radians);
            cos = (float)Math.Cos(radians);
        }

        public static Matrix3D PerspectiveTransform(double depth)
        {
            return new Matrix3D
            {
                M34 = -1 / (float)depth
            };
        }

        public static Matrix3D operator *(Matrix3D l, Matrix3D r)
        {
            return new Matrix3D
            {
                M11 = l.M11 * r.M11 + l.M12 * r.M21 + l.M13 * r.M31 + l.M14 * r.M41,
                M12 = l.M11 * r.M12 + l.M12 * r.M22 + l.M13 * r.M32 + l.M14 * r.M42,
                M13 = l.M11 * r.M13 + l.M12 * r.M23 + l.M13 * r.M33 + l.M14 * r.M43,
                M14 = l.M11 * r.M14 + l.M12 * r.M24 + l.M13 * r.M34 + l.M14 * r.M44,

                M21 = l.M21 * r.M11 + l.M22 * r.M21 + l.M23 * r.M31 + l.M24 * r.M41,
                M22 = l.M21 * r.M12 + l.M22 * r.M22 + l.M23 * r.M32 + l.M24 * r.M42,
                M23 = l.M21 * r.M13 + l.M22 * r.M23 + l.M23 * r.M33 + l.M24 * r.M43,
                M24 = l.M21 * r.M14 + l.M22 * r.M24 + l.M23 * r.M34 + l.M24 * r.M44,

                M31 = l.M31 * r.M11 + l.M32 * r.M21 + l.M33 * r.M31 + l.M34 * r.M41,
                M32 = l.M31 * r.M12 + l.M32 * r.M22 + l.M33 * r.M32 + l.M34 * r.M42,
                M33 = l.M31 * r.M13 + l.M32 * r.M23 + l.M33 * r.M33 + l.M34 * r.M43,
                M34 = l.M31 * r.M14 + l.M32 * r.M24 + l.M33 * r.M34 + l.M34 * r.M44,

                M41 = l.M41 * r.M11 + l.M42 * r.M21 + l.M43 * r.M31 + l.M44 * r.M41,
                M42 = l.M41 * r.M12 + l.M42 * r.M22 + l.M43 * r.M32 + l.M44 * r.M42,
                M43 = l.M41 * r.M13 + l.M42 * r.M23 + l.M43 * r.M33 + l.M44 * r.M43,
                M44 = l.M41 * r.M14 + l.M42 * r.M24 + l.M43 * r.M34 + l.M44 * r.M44
            };
        }
    }
}
