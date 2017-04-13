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

            string resourceID = "SkiaSharpFormsDemos.Media.FacePalm.jpg";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            using (SKManagedStream skStream = new SKManagedStream(stream))
            {
                bitmap = SKBitmap.Decode(skStream);
            }
        }

        void sliderValueChanged(object sender, ValueChangedEventArgs args)
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

            float xCenter = info.Width / 2;
            float yCenter = info.Height / 2;

            double xRadians = Math.PI * xRotateSlider.Value / 180;
            double yRadians = Math.PI * yRotateSlider.Value / 180;
            double zRadians = Math.PI * zRotateSlider.Value / 180;
            float depth = Math.Max(bitmap.Width, bitmap.Height);

            // Translate center to origin
            SKMatrix matrix = SKMatrix.MakeTranslation(-xCenter, -yCenter);

            // Rotation around the X-axis
            SKMatrix.PostConcat(ref matrix, new SKMatrix
                {
                    ScaleX = 1,
                    ScaleY = (float)Math.Cos(xRadians),
                    Persp1 = (float)Math.Sin(xRadians) / depth,
                    Persp2 = 1
                });

            // Rotate around the Y-axis
            SKMatrix.PostConcat(ref matrix, new SKMatrix
                {
                    ScaleX = (float)Math.Cos(yRadians),
                    ScaleY = 1,
                    Persp0 = (float)Math.Sin(yRadians) / depth,
                    Persp2 = 1
                });

            // Rotate around the Z-axis
            SKMatrix.PostConcat(ref matrix, 
                SKMatrix.MakeRotationDegrees((float)zRotateSlider.Value));
/*
            // Apply perspective
            SKMatrix.PostConcat(ref matrix, new SKMatrix
                {
                    ScaleX = 1,
                    ScaleY = 1,
                    Persp1 = -1 / depth,
                    Persp2 = 1
                });
*/
            // Translate back to center
            SKMatrix.PostConcat(ref matrix, SKMatrix.MakeTranslation(xCenter, yCenter));

            //threeDView.RotateXDegrees((float)xRotateSlider.Value);
            //threeDView.RotateYDegrees((float)yRotateSlider.Value);
            //threeDView.RotateZDegrees((float)zRotateSlider.Value);
            //threeDView.Translate(xCenter, yCenter, 0);

            //            threeDView.ApplyToCanvas(canvas);

            //     canvas.SetMatrix(matrix44.Matrix);

            canvas.SetMatrix(matrix);

            float xBitmap = xCenter - bitmap.Width / 2;
            float yBitmap = yCenter - bitmap.Height / 2;

            canvas.DrawBitmap(bitmap, xBitmap, yBitmap);
        }
    }
}
