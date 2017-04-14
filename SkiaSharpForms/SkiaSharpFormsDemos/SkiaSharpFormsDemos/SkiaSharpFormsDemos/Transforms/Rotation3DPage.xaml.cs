using System;
using System.IO;
using System.Reflection;

using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpFormsDemos.Transforms
{
    public partial class Rotation3DPage : ContentPage
    {
        SKBitmap bitmap;
        //     SK3dView threeDView = new SK3dView();

     //   SKMatrix44 matrix44 = SKMatrix44.CreateIdentity();

        public Rotation3DPage()
        {
            InitializeComponent();

            string resourceID = "SkiaSharpFormsDemos.Media.SeatedMonkey.jpg";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            using (SKManagedStream skStream = new SKManagedStream(stream))
            {
                bitmap = SKBitmap.Decode(skStream);
            }
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            if (canvasView != null)
            {
                canvasView.InvalidateSurface();
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            // Find center of canvas
            float xCenter = info.Width / 2;
            float yCenter = info.Height / 2;

            // Translate center to origin
            SKMatrix matrix = SKMatrix.MakeTranslation(-xCenter, -yCenter);

            // Calculate composite 3D transforms
            Matrix3D matrix3d = Matrix3D.RotationX(xRotateSlider.Value) *
                                Matrix3D.RotationY(yRotateSlider.Value) *
                                Matrix3D.RotationZ(zRotateSlider.Value) *
                                Matrix3D.PerspectiveTransform(depthSlider.Value);

            // Concatenate with 2D matrix
            SKMatrix.PostConcat(ref matrix, matrix3d.Matrix2D);

            // Translate back to center
            SKMatrix.PostConcat(ref matrix, 
                SKMatrix.MakeTranslation(xCenter, yCenter));

            // Set the matrix and display the bitmap
            canvas.SetMatrix(matrix);
            float xBitmap = xCenter - bitmap.Width / 2;
            float yBitmap = yCenter - bitmap.Height / 2;
            canvas.DrawBitmap(bitmap, xBitmap, yBitmap);
        }
    }
}
