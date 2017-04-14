using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpFormsDemos.Transforms
{
    public partial class TestPerspectivePage : ContentPage
    {
        SKBitmap bitmap;

        public TestPerspectivePage()
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

        void OnPersp0SliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            Slider slider = (Slider)sender;
            persp0Label.Text = String.Format("Persp0 = {0:F4}", slider.Value);
            canvasView.InvalidateSurface();
        }

        void OnPersp1SliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            Slider slider = (Slider)sender;
            persp1Label.Text = String.Format("Persp1 = {0:F4}", slider.Value);
            canvasView.InvalidateSurface();
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            // Calculate perspective matrix
            SKMatrix perspectiveMatrix = SKMatrix.MakeIdentity();
            perspectiveMatrix.Persp0 = (float)persp0Slider.Value / 100;
            perspectiveMatrix.Persp1 = (float)persp1Slider.Value / 100;

            // Coordinates to center bitmap on canvas
            float x = (info.Width - bitmap.Width) / 2;
            float y = (info.Height - bitmap.Height) / 2;

            // Center of bitmap
            float xCenter = x + bitmap.Width / 2;
            float yCenter = y + bitmap.Height / 2;

            SKMatrix matrix = SKMatrix.MakeTranslation(-xCenter, -yCenter);
            SKMatrix.PostConcat(ref matrix, perspectiveMatrix);
            SKMatrix.PostConcat(ref matrix, SKMatrix.MakeTranslation(xCenter, yCenter));

            canvas.SetMatrix(matrix);
            canvas.DrawBitmap(bitmap, x, y);
        }
    }
}