using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms;
using St_Dogmaels.Extensions;

namespace St_Dogmaels.Models
{
	public class MapView : ContentView
	{
        //double x, y;
        double currentScale = .5;
        double startScale = .5;
        double xOffset = 0;
        double yOffset = 0;

        public Position Center { get; set; }

        public MapView ()
		{
           
            //Content = new StackLayout {
            //	Children = {
            //		new Label { Text = "Welcome to Xamarin.Forms!" }
            //	}
            //};

            // Set PanGestureRecognizer.TouchPoints to control the 
            // number of touch points needed to pan
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);

            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinchGesture);
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {

                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.

                    Debug.WriteLine("DEBUG:Tap Running" + " Content.Width:" + Content.Width.ToString() + " Content.Height:" + Content.Height.ToString());
                    Debug.WriteLine("                 " + " ScreenWidth:" + App.ScreenWidth.ToString() + " ScreenHeight:" + App.ScreenHeight.ToString());
                    Debug.WriteLine("                 " + " e.TotalX:" + e.TotalX.ToString() + " e.TotalY:" + e.TotalY.ToString());
                    Debug.WriteLine("                 " + " xOffset:" + xOffset.ToString() + " yOffset:" + yOffset.ToString());
                    Debug.WriteLine("                 " + "Math.Min(0, xOffset + e.TotalX)=" + Math.Min(0, xOffset + e.TotalX) + "  -Math.Abs(Content.Width - App.ScreenWidth)=" + (-Math.Abs(Content.Width - App.ScreenWidth)).ToString());
                    Debug.WriteLine("                 " + "Math.Min(0, yOffset + e.TotalY)=" + Math.Min(0, yOffset + e.TotalY) + "  -Math.Abs(Content.Height - App.ScreenHeight)=" + (-Math.Abs(Content.Height - App.ScreenHeight)).ToString());
                    Content.TranslationX = Math.Max(Math.Min(0, xOffset + e.TotalX), -Math.Abs(Content.Width - App.ScreenWidth));
                    Content.TranslationY = Math.Max(Math.Min(0, yOffset + e.TotalY), -Math.Abs(Content.Height - App.ScreenHeight));
                    Debug.WriteLine("-----------------" + " Content.TranslationX:" + Content.TranslationX.ToString() + " Content.TranslationY:" + Content.TranslationY.ToString());

                    break;

                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    xOffset = Content.TranslationX;
                    yOffset = Content.TranslationY;
                    Debug.WriteLine("DEBUG:Tap Completed" + " Content.TranslationX:" + Content.TranslationX.ToString() + " Content.TranslationY:" + Content.TranslationY.ToString());
                    break;
            }
        }

        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(.5, currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin.
                Content.TranslationX = targetX.Clamp(-Content.Width * (currentScale - 1), 0);
                Content.TranslationY = targetY.Clamp(-Content.Height * (currentScale - 1), 0);

                // Apply scale factor
                Content.Scale = currentScale;
                Debug.WriteLine("DEBUG:Pinch Running" + " Content.Scale:" + Content.Scale.ToString());

            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
                Debug.WriteLine("DEBUG:Pinch Complet" + " Content.Scale:" + Content.Scale.ToString());
            }
        }
    }
}