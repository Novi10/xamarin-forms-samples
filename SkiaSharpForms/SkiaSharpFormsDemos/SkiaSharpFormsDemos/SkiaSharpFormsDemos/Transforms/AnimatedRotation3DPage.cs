using System;

using Xamarin.Forms;

using SkiaSharp;

using SkiaSharp.Views.Forms;

namespace SkiaSharpFormsDemos.Transforms
{
    public class AnimatedRotation3DPage : ContentPage
    {
        SKCanvasView canvasView;
        float xRotationDegrees, yRotationDegrees, zRotationDegrees;
        string text = "SkiaSharp"; 
        SKPaint textPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            TextSize = 100,
            StrokeWidth = 3,
        };
        SKRect textBounds;

        public AnimatedRotation3DPage()
        {
            Title = "Animated Rotation 3D";

            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;

            // Measure the text
            textPaint.MeasureText(text, ref textBounds);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            new Animation((value) => xRotationDegrees = 360 * (float)value).
                Commit(this, "xRotationAnimation", length: 5000, repeat: () => true);

            new Animation((value) => yRotationDegrees = 360 * (float)value).
                Commit(this, "yRotationAnimation", length: 7000, repeat: () => true);

            new Animation((value) =>
            {
                zRotationDegrees = 360 * (float)value;
                canvasView.InvalidateSurface();
            }).Commit(this, "zRotationAnimation", length: 11000, repeat: () => true);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.AbortAnimation("xRotationAnimation");
            this.AbortAnimation("yRotationAnimation");
            this.AbortAnimation("zRotationAnimation");
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

            // Scale so text fits
            float scale = Math.Min(info.Width / textBounds.Width, 
                                   info.Height / textBounds.Height);
            SKMatrix.PostConcat(ref matrix, SKMatrix.MakeScale(scale, scale));

            // Calculate composite 3D transforms
            float depth = 0.75f * scale * textBounds.Width;

            Matrix3D matrix3d = Matrix3D.RotationX(xRotationDegrees) *
                                Matrix3D.RotationY(yRotationDegrees) *
                                Matrix3D.RotationZ(zRotationDegrees) *
                                Matrix3D.PerspectiveTransform(depth);

            // Concatenate with 2D matrix
            SKMatrix.PostConcat(ref matrix, matrix3d.Matrix2D);

            // Translate back to center
            SKMatrix.PostConcat(ref matrix,
                SKMatrix.MakeTranslation(xCenter, yCenter));

            // Set the matrix and display the text
            canvas.SetMatrix(matrix);
            float xText = xCenter - textBounds.MidX;
            float yText = yCenter - textBounds.MidY;
            canvas.DrawText(text, xText, yText, textPaint);
        }
    }
}