
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using St_Dogmaels.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using SkiaSharp;
using SkiaSharp.Views;
using SkiaSharp.Views.Forms;
using Acr;
using St_Dogmaels.Extensions;
using System.Diagnostics;

using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using St_Dogmaels.EventArg;

namespace St_Dogmaels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private const float touchRange = 20.0f;

        List<SKBitmap> bitmaps;
        List<MapTile> tiles;
        
        List<Marker> markers = new List<Marker>();

        SKPoint lastPoint = new SKPoint();
        TileColRow lastTileColRow = null;
        SKCanvas canvas;
        
        float xOffset;
        float yOffset;

        

        public MapPage()
        {

            InitializeComponent();

            Debug.WriteLine("*************Initialize map");

           
                
            
            foreach (House house in App.houses)
            {
                Marker marker = new Marker();
                marker.Id = house.ID;
                marker.Title = house.Title;
                marker.Subtitle = house.Subtitle;
                marker.Position = new Position(house.Lat, house.Lon);
                marker.Point = new SKPoint(-1, -1);
                if (house.Tags == null)
                {
                        
                    marker.Type = MarkerType.House;
                    markers.Add(marker);
              
                }
                else if (house.Tags.Contains("landscape"))
                {
                        
                    marker.Type = MarkerType.Landscape;
                    markers.Add(marker);

                }
                else if (house.Tags.Contains("gardens"))
                {
                        
                    marker.Type = MarkerType.Gardens;
                    markers.Add(marker);
      
                }
                else
                {
                       
                    marker.Type = MarkerType.House;
                    markers.Add(marker);
                    
                }
                    
                    
            }
            

        }

        private void GetWalkMarkers()
        {
            int n = 1;
            for(int i=markers.Count-1; i>=0; i--)
            {
                if (markers[i].Type == MarkerType.Step)
                    markers.RemoveAt(i);
            }
            if (App.Walks.Count > 0)
            {


                for (int i = 0; i < App.Walks.Count; i++)
                {
                    for (int j = 0; j < App.Walks[i].Steps.Count; j++)
                    {
                        Marker marker = new Marker();
                        marker.Id = "W";
                        marker.Type = MarkerType.Step;
                        marker.Number = (n++).ToString();
                        marker.Title = App.Walks[i].Steps[j].Title;
                        marker.Subtitle = App.Walks[i].Steps[j].Subtitle;
                        marker.Position = new Position(App.Walks[i].Steps[j].Latitude, App.Walks[i].Steps[j].Longitude);
                        marker.Point = new SKPoint(-1, -1);
                        markers.Add(marker);

                    }

                }

            }
        
        }

        public void OnCentreMap(object sender, CentreMapEventArgs cm)
        {
            CentreMap(cm.Latitude, cm.Longitude, cm.Title, cm.Subtitle);

        }

        private void CentreMap(double latitude, double longitude, string title, string subtitle)
        {
            App.PreviousCentre.Latitude = App.CurrentCentre.Latitude;
            App.PreviousCentre.Longitude = App.CurrentCentre.Longitude;
            App.CurrentCentre.Longitude = longitude;
            App.CurrentCentre.Latitude = latitude;

            Marker marker = new Marker();
            marker.Id = "S";
            marker.Type = MarkerType.Search;
            marker.Number = " ";
            marker.Title = title;
            marker.Subtitle = subtitle;
            marker.Position = new Position(latitude, longitude);
            marker.Point = new SKPoint(-1, -1);
            markers.Add(marker);



            canvasView.InvalidateSurface();

        }



        private void OnCanvasViewPaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            Debug.WriteLine("*************PaintSUrface map");

            bitmaps = new List<SKBitmap>();

            GetWalkMarkers();
           

            var newTiles = GetTiles();
            if (newTiles != null)
            {
                tiles = newTiles;
                Debug.WriteLine("No. of Tiles:" + tiles.Count.ToString());
            }

            
            if (tiles.Count != 49)
            {
                lastTileColRow.Column = 0;
                lastTileColRow.Row = 0;
                App.CurrentCentre.Latitude = App.PreviousCentre.Latitude;
                App.CurrentCentre.Longitude = App.PreviousCentre.Longitude;
                tiles = GetTiles();
            }
            for (int i = 0; i < tiles.Count; i++)
            {
                MemoryStream stream = new MemoryStream(tiles[i].Tile_data);
                bitmaps.Add(SKBitmap.Decode(stream));
            }


            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            canvas = surface.Canvas;
            SKRect deviceClipBounds = canvas.DeviceClipBounds;
            SKRectI bounds = new SKRectI();
            bool gotBounds = canvas.GetDeviceClipBounds(out bounds);
         
            

            canvas.Clear(); 
            canvas.Scale(App.ZoomScale);
            
           

            canvas.DrawColor(SKColors.White);
            int b = 0;

            double topLeftLat = tiles[0].Tile_lat;
            double topLeftLon = tiles[0].Tile_lon;

            
            double distanceToCentreLat = topLeftLat - App.CurrentCentre.Latitude;
            double pixelsToCentreLat = distanceToCentreLat / App.CurrentLatDegreesPerPixel;
            double yOff = (pixelsToCentreLat ) - (bounds.MidY / App.ZoomScale);
            
            

            double distanceToCentreLon = Math.Abs(topLeftLon - App.CurrentCentre.Longitude);
            double pixelsToCentreLon = distanceToCentreLon / App.CurrentLonDegreesPerPixel;
            double xOff = (pixelsToCentreLon) - (bounds.MidX / App.ZoomScale);




            xOffset = Convert.ToSingle(xOff);
            yOffset = Convert.ToSingle(yOff);

          
           
            SKPaint bitmapPaint = new SKPaint();
            bitmapPaint.FilterQuality = SKFilterQuality.High;
    

            for (int x = 0; x < tiles.Count / 7; x++) //
            {
                for (int y = 0; y < tiles.Count / 7; y++) //
                {
                    
                    float fx = Convert.ToSingle((x * 256) - (xOffset));
                    float fy = Convert.ToSingle((y * 256) - (yOffset));
                    
                    SKImage image = SKImage.FromBitmap(bitmaps[b++]);
                    canvas.DrawImage(image, fx, fy, bitmapPaint);
                    

                }

               
            }

            //canvas.Translate(-xOffset, -yOffset);
            
            foreach (Marker marker in markers)
            {
                switch (marker.Type)
                {
                    case MarkerType.Facility:
                        if (App.LayerFacilities)
                            DrawMarker(canvas, marker);
                        break;

                    case MarkerType.Gardens:
                        if (App.LayerGardens)
                            DrawMarker(canvas, marker);
                        break;

                    case MarkerType.House:
                        if (App.LayerHouses)
                            DrawMarker(canvas, marker);
                        break;

                    case MarkerType.Landscape:
                        if (App.LayerLandscape)
                            DrawMarker(canvas, marker);
                        break;
                    case MarkerType.Poi:
                        if (App.LayerPois)
                            DrawMarker(canvas, marker);
                        break;

                    case MarkerType.Step:
                        if (App.LayerSteps )
                            DrawMarker(canvas, marker);
                        break;

                    case MarkerType.Search:
  
                        DrawMarker(canvas, marker);

                        break;

                }
                
            }

            

        }

        private void DrawMarker(SKCanvas canvas, Marker marker)
        {
            SKRectI bounds = new SKRectI();
            double topLeftLat = tiles[0].Tile_lat;
            double topLeftLon = tiles[0].Tile_lon;

            bool gotBounds = canvas.GetDeviceClipBounds(out bounds);

            double distanceFromCentreLat = marker.Position.Latitude - App.CurrentCentre.Latitude;
            double pixelsFromCentreLat = distanceFromCentreLat / App.CurrentLatDegreesPerPixel;
            double yOff = (pixelsFromCentreLat * -1) + (bounds.MidY / App.ZoomScale);

            double distanceFromCentreLon = marker.Position.Longitude - App.CurrentCentre.Longitude;
            double pixelsFromCentreLon = distanceFromCentreLon / App.CurrentLonDegreesPerPixel;
            double xOff = pixelsFromCentreLon + (bounds.MidX / App.ZoomScale);

            float x = Convert.ToSingle(xOff) ;
            float y = Convert.ToSingle(yOff) ;

            SKColor pointerColor = SKColors.White;
            marker.Point = new SKPoint(x, y);
            SKPaint circlePaint = new SKPaint();
            switch (marker.Type)
            { 
                case MarkerType.Step:
                    // Create circle fill

                    
                    circlePaint.Style = SKPaintStyle.Fill;
                    circlePaint.Color = SKColors.White;
                    circlePaint.IsAntialias = true;

                    canvas.DrawCircle(x, y - 30, 18, circlePaint);

               

                    // circle stroke
                    circlePaint.Style = SKPaintStyle.Stroke;
                    circlePaint.StrokeWidth = 5.0f;
                    circlePaint.Color = SKColors.Red;
                    circlePaint.IsAntialias = true;
                    canvas.DrawCircle(x, y - 30, 18, circlePaint);

                    // Add Text
                    SKPaint textPaint = new SKPaint()
                    {
                        TextSize = 16.0f,
                        IsAntialias = true,
                        Color = SKColors.Black,
                        Style = SKPaintStyle.StrokeAndFill, 
                        StrokeWidth = 1,
                        TextAlign = SKTextAlign.Center
                    };
                    canvas.DrawText(marker.Number, x, y - 30 + (textPaint.TextSize / 2) - 2, textPaint);
                
                    pointerColor = SKColors.Red;
                    break;

                case MarkerType.Search:
                    // Create circle fill


                    circlePaint.Style = SKPaintStyle.Fill;
                    circlePaint.Color = SKColors.Blue;
                    circlePaint.IsAntialias = true;

                    canvas.DrawCircle(x, y - 30, 18, circlePaint);



                    // circle stroke
                    circlePaint.Style = SKPaintStyle.Stroke;
                    circlePaint.StrokeWidth = 5.0f;
                    circlePaint.Color = SKColors.Blue;
                    circlePaint.IsAntialias = true;
                    canvas.DrawCircle(x, y - 30, 18, circlePaint);
                    pointerColor = SKColors.Blue;
                    break;
                case MarkerType.Gardens:
                    // Create circle fill
                    circlePaint.Style = SKPaintStyle.Fill;
                    circlePaint.Color = SKColors.LightBlue;
                    circlePaint.IsAntialias = true;
                    canvas.DrawCircle(x, y - 30, 18, circlePaint);
                    // circle stroke
                    circlePaint.Style = SKPaintStyle.Stroke;
                    circlePaint.StrokeWidth = 5.0f;
                    circlePaint.Color = SKColors.Green;
                    circlePaint.IsAntialias = true;
                    canvas.DrawCircle(x, y - 30, 18, circlePaint);
                    // draw flower
                    circlePaint.Style = SKPaintStyle.Fill;
                    //stalk
                    circlePaint.Color = SKColors.Green;
                    canvas.DrawRect(x - 1, y - 28, 2, 12, circlePaint);
                    //petals
                    circlePaint.Color = SKColors.Yellow;
                    canvas.DrawCircle(x - 4, y - 34, 6, circlePaint);
                    canvas.DrawCircle(x + 4, y - 34, 6, circlePaint);
                    canvas.DrawCircle(x - 4, y - 30, 6, circlePaint);
                    canvas.DrawCircle(x + 4, y - 30, 6, circlePaint);
                    //centre
                    circlePaint.Color = SKColors.Orange;
                    canvas.DrawCircle(x, y - 32, 4, circlePaint);
                   
                    pointerColor = SKColors.Green;
                    break;
                case MarkerType.Landscape:
                    float x1 = x - 18;
                    float y1 = y - 23;
                    float x2 = x - 12;
                    float y2 = y - 29;
                    float x3 = x - 8;
                    float y3 = y - 34;
                    float x4 = x - 5;
                    float y4 = y - 30;
                    float x5 = x + 5;
                    float y5 = y - 26;
                    float x6 = x + 18;
                    float y6 = y - 22;

                    // Create circle fill
                    circlePaint.Style = SKPaintStyle.Fill;
                    circlePaint.Color = SKColors.LightBlue;
                    circlePaint.IsAntialias = true;
                    canvas.DrawCircle(x, y - 30, 18, circlePaint);
                    circlePaint.Color = SKColors.SandyBrown;
                    circlePaint.Style = SKPaintStyle.Stroke;
                    circlePaint.StrokeWidth = 5;
                    SKPath landPath1 = new SKPath();
                    landPath1.MoveTo(x1, y1);
                    landPath1.LineTo(x2, y2);
                    landPath1.LineTo(x3, y3);
                    landPath1.LineTo(x4, y4);
                    landPath1.LineTo(x5, y5);
                    landPath1.LineTo(x6, y6);
                    canvas.DrawPath(landPath1, circlePaint);
                    circlePaint.Color = SKColors.Green;
                    SKPath landPath2 = new SKPath();
                    landPath2.MoveTo(x1 + 3, y1 + 5);
                    landPath2.LineTo(x2, y2 + 5);
                    landPath2.LineTo(x3, y3 + 5);
                    landPath2.LineTo(x4, y4 + 5);
                    landPath2.LineTo(x5, y5 + 5);
                    landPath2.LineTo(x6 - 2, y6 + 4);
                    canvas.DrawPath(landPath2, circlePaint);
                    circlePaint.Color = SKColors.DarkGreen;
                    circlePaint.Style = SKPaintStyle.StrokeAndFill;
                    SKPath landPath3 = new SKPath();
                    landPath3.MoveTo(x1 + 8, y1 + 9);
                    landPath3.LineTo(x2, y2 + 8);
                    landPath3.LineTo(x3, y3 + 10);
                    landPath3.LineTo(x4, y4 + 10);
                    landPath3.LineTo(x5, y5 + 10);
                    landPath3.LineTo(x6 - 6, y6 + 7);
                    canvas.DrawPath(landPath3, circlePaint);
                    // circle stroke
                    circlePaint.Style = SKPaintStyle.Stroke;
                    circlePaint.StrokeWidth = 5.0f;
                    circlePaint.Color = SKColors.Green;
                    circlePaint.IsAntialias = true;
                    canvas.DrawCircle(x, y - 30, 18, circlePaint);


                    pointerColor = SKColors.Green;
                    break;
                case MarkerType.House:
                    // create house
                    // roof
                    SKPath roofPath = new SKPath();
                    roofPath.MoveTo(x - 13, y - 30);
                    roofPath.LineTo(x - 11, y - 41);
                    roofPath.LineTo(x + 10, y - 41);
                    roofPath.LineTo(x + 12, y - 30);
                    roofPath.LineTo(x - 13, y - 30);


                    SKPaint roofPaint = new SKPaint
                    {
                        Style = SKPaintStyle.StrokeAndFill,
                        Color = SKColors.SlateGray,
                        IsAntialias = true,
                        StrokeWidth = 1
                    };
                    canvas.DrawPath(roofPath, roofPaint);

                    // building 
                    SKPaint rectPaint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.Firebrick,
                        IsAntialias = true
                    };
                    canvas.DrawRect(x - 11, y - 30, 21,21, rectPaint);
                    pointerColor = SKColors.Firebrick;

                    // door
                    SKPaint doorPaint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.DarkBlue
                    };
                    canvas.DrawRect(x - 2, y - 16, 4, 12, doorPaint);
                    // window
                    SKPaint windowPaint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.LightCyan
                    };
                    canvas.DrawRect(x - 8, y - 24, 5, 4, windowPaint);
                    canvas.DrawRect(x + 2, y - 24, 5, 4, windowPaint);
                    break;

                    

            }
            //Add pointer

            // Create the path
            SKPath pointerPath = new SKPath();         
            pointerPath.MoveTo(x, y);
            pointerPath.LineTo(x - 4, y - 9);
            pointerPath.LineTo(x + 4, y - 9);
            pointerPath.LineTo(x, y);
            // Create two SKPaint objects
            SKPaint pointerPaint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                Color = pointerColor,
                IsAntialias = true,
                StrokeWidth = 1
            };
            canvas.DrawPath(pointerPath, pointerPaint);

           


        }

        private List<MapTile> GetTiles()
        {
            TileColRow tileColRow = WorldToTilePos(App.CurrentCentre.Longitude, App.CurrentCentre.Latitude, App.ZoomLevel);

            if (lastTileColRow != null)
            {
                if (lastTileColRow.Column == tileColRow.Column && lastTileColRow.Row == tileColRow.Row)
                {
                    return null;
                }
            }
            lastTileColRow = tileColRow;
            int colFrom = tileColRow.Column - 3;
            int colTo = tileColRow.Column + 3;
            int rowFrom = tileColRow.Row - 3;
            int rowTo = tileColRow.Row + 3;
            string[] param = { App.ZoomLevel.ToString(), colFrom.ToString(), colTo.ToString(), rowFrom.ToString(), rowTo.ToString() };

            String query = "SELECT * FROM maptiles WHERE zoom_level = ? AND (tile_column BETWEEN ? AND ?) AND (tile_row BETWEEN ? AND ?) ORDER BY tile_column, tile_row";
            List<MapTile> tiles = App.mapDB.Query<MapTile>(query, param);
            return tiles;
        }

        public TileColRow WorldToTilePos(double lon, double lat, int zoom)
        {
            TileColRow p = new TileColRow();
            p.Column = Convert.ToInt32(Math.Floor((lon + 180.0) / 360.0 * (1 << zoom)));
            p.Row = Convert.ToInt32(Math.Floor((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) +
                1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << zoom)));

            return p;
        }

        private void Zoom_Out_Clicked(object sender, EventArgs e)
        {
            if (App.ZoomScale > App.ZoomMinium)
            {
               
                App.ZoomScale -= 0.1f;
      
                canvasView.InvalidateSurface();
                this.ToolbarItems[0].Text = App.ZoomScale.ToString();
            }
        }

        private void Zoom_In_Clicked(object sender, EventArgs e)
        {
            if (App.ZoomScale < App.ZoomMaximum)
            {

                App.ZoomScale += 0.1f;
     
                canvasView.InvalidateSurface();
                this.ToolbarItems[0].Text = App.ZoomScale.ToString();
               
            }
        }

        private void OnTouch(object sender, SKTouchEventArgs e)
        {
            // handle touch events
            Debug.WriteLine("OnTouch ID:" + e.Id.ToString() + " Type:" + e.ActionType.ToString() + " X:" + e.Location.X.ToString() + " Y:" + e.Location.Y.ToString());
            double xOffsetD;
            double yOffsetD;

            switch (e.ActionType)
            {
                

                case SKTouchAction.Pressed:
                    if (e.Id == 0)
                    {
                        App.Pressed = e.Location;
                        lastPoint = e.Location;
                        App.Pressed1 = new SKPoint(-1f, -1f);
                        App.ReleasedCount = 0;
                        Debug.WriteLine("Pressed:" + App.Pressed.ToString() + " Pressed1:" + App.Pressed1.ToString());
                    }
                    else
                    {
                        App.Pressed1 = e.Location;
                       
                        Debug.WriteLine("Pressed:" + App.Pressed.ToString() + " Pressed1:" + App.Pressed1.ToString());
                    }
                    
                    break;

                case SKTouchAction.Entered:
                    
                    break;

                case SKTouchAction.Moved:
                    xOffsetD = (lastPoint.X  - e.Location.X)  * App.CurrentLonDegreesPerPixel;
                    yOffsetD = (lastPoint.Y  - e.Location.Y) * App.CurrentLatDegreesPerPixel;

                    App.PreviousCentre.Latitude = App.CurrentCentre.Latitude;
                    App.PreviousCentre.Longitude = App.CurrentCentre.Longitude;
                    App.CurrentCentre.Longitude += xOffsetD;
                    App.CurrentCentre.Latitude -= yOffsetD;
                    lastPoint = e.Location;
                    
                    canvasView.InvalidateSurface();

                    break;

                case SKTouchAction.Released:
                    if (App.Pressed1.X == -1)
                    {

                        if (e.Location.X == App.Pressed.X && e.Location.Y == App.Pressed.Y)
                        {
                            DisplayMarker();
                        }
                        else
                        {

                            xOffsetD = (lastPoint.X - e.Location.X) * App.CurrentLonDegreesPerPixel;
                            yOffsetD = (lastPoint.Y - e.Location.Y) * App.CurrentLatDegreesPerPixel;

                            App.PreviousCentre.Latitude = App.CurrentCentre.Latitude;
                            App.PreviousCentre.Longitude = App.CurrentCentre.Longitude;
                            App.CurrentCentre.Longitude += xOffsetD;
                            App.CurrentCentre.Latitude -= yOffsetD;
                            //SKCanvasView canvasView = this.FindByName<SKCanvasView>("canvasView");
                            canvasView.InvalidateSurface();
                        }
                        App.Pressed1 = new SKPoint(-1, -1);
                        App.ReleasedCount = 0;
                    }
                    else
                    {
                        App.ReleasedCount++;
                        Debug.WriteLine("Released:" + App.Pressed.ToString() + " Pressed1:" + App.Pressed1.ToString());
                        if (App.ReleasedCount == 2)
                        {
                            Debug.WriteLine("Released2:" + App.Pressed.ToString() + " Pressed1:" + App.Pressed1.ToString());
                            App.ReleasedCount = 0;
                            float scaleX = App.Pressed1.X / App.Pressed.X;
                            float scaleY = App.Pressed1.Y / App.Pressed.Y;
                            Debug.WriteLine("scalex" + scaleX.ToString() + " scaley" + scaleY.ToString() + "Pressed" + App.Pressed.ToString() + "Pressed1" + App.Pressed1.ToString());
                        }
                    }
                    break;
                case SKTouchAction.Exited:
                    
                    break;
            }
            e.Handled = true;
        }

        

        private void DisplayMarker()
        {
            foreach (Marker marker in markers)
            {
                //Debug.WriteLine("DisplayMarker X:" + marker.Point.X.ToString() + " PressedX:" + (App.Pressed.X / App.ZoomScale).ToString() + " Y:" + marker.Point.Y.ToString() + " PressedY:" + (App.Pressed.Y / App.ZoomScale).ToString());
                if ((marker.Point.X >= (App.Pressed.X / App.ZoomScale) - touchRange && marker.Point.X <= (App.Pressed.X / App.ZoomScale) + touchRange) && (marker.Point.Y - 30 >= (App.Pressed.Y / App.ZoomScale) - touchRange && marker.Point.Y -30 <= (App.Pressed.Y / App.ZoomScale) + touchRange)) 
                {
                    

                    HousesPopup(marker.Id, marker.Title, marker.Subtitle);
                    break;
                }
                
            }
        }

        private async void HousesPopup(string id, string title, string subtitle)
        {
            await Navigation.PushPopupAsync(new HousePopup(id, title, subtitle));
        }

        private async void Layer_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MapLayersPage());
        }

        async void SearchButtonClicked(object sender, EventArgs e)
        {
            
                SearchPage searchPage = new SearchPage();


                searchPage.CentreMap += OnCentreMap;

                await Navigation.PushModalAsync(searchPage, true);
            
        }

        private void ClearSearchesClicked(object sender, EventArgs e)
        {

        }

        private void ClearWalksClicked(object sender, EventArgs e)
        {
            App.Walks.Clear();

            canvasView.InvalidateSurface();
        }





        //private void OnTouchEffectAction(object sender, ViewModel.TouchActionEventArgs args)
        //{
        //        Debug.WriteLine("ID:" + args.Id.ToString() + " Type:" + args.Type.ToString() + " X:" + args.Location.X.ToString() + " Y:" + args.Location.Y.ToString());
        //        switch (args.Type)
        //        {
        //            case TouchActionType.Pressed:
        //                //AddToList(args.Id);
        //                App.PressedX = args.Location.X;
        //                App.PressedY = args.Location.Y;
        //                Debug.WriteLine("Pressed: " + args.Location.X.ToString() + "," + args.Location.Y.ToString());
        //                break;

        //            case TouchActionType.Entered:
        //                Debug.WriteLine("");
        //                if (args.IsInContact)
        //                {
        //                    //AddToList(args.Id);
        //                }
        //                break;

        //            case TouchActionType.Moved:
        //                Debug.WriteLine("Moved: " + args.Location.X.ToString() + "," + args.Location.Y.ToString());
        //                break;

        //            case TouchActionType.Released:
        //                Debug.WriteLine("Released: " + args.Location.X.ToString() + "," + args.Location.Y.ToString());
        //                double xOffset = (App.PressedX - args.Location.X) * App.CurrentLonDegreesPerPixel;
        //                double yOffset = (App.PressedY - args.Location.Y) * App.CurrentLatDegreesPerPixel;

        //                App.PreviousCentre.Latitude = App.CurrentCentre.Latitude;
        //                App.PreviousCentre.Longitude = App.CurrentCentre.Longitude;
        //                App.CurrentCentre.Longitude += xOffset;
        //                App.CurrentCentre.Latitude -= yOffset;
        //                SKCanvasView canvasView = this.FindByName<SKCanvasView>("canvasView");
        //                canvasView.InvalidateSurface();
        //                break;

        //            case TouchActionType.Exited:
        //                Debug.WriteLine("Exited: " + args.Location.X.ToString() + "," + args.Location.Y.ToString());
        //                break;
        //        }
        //    }

        //    private void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        //    {
        //        if (e.Status == GestureStatus.Started)
        //        {
        //            // Store the current scale factor applied to the wrapped user interface element,
        //            // and zero the components for the center point of the translate transform.
        //            startScale = Content.Scale;
        //            Content.AnchorX = 0;
        //            Content.AnchorY = 0;
        //        }
        //        if (e.Status == GestureStatus.Running)
        //        {
        //            // Calculate the scale factor to be applied.
        //            currentScale += (e.Scale - 1) * startScale;
        //            currentScale = Math.Max(1, currentScale);

        //            // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
        //            // so get the X pixel coordinate.
        //            double renderedX = Content.X + xOffset;
        //            double deltaX = renderedX / Width;
        //            double deltaWidth = Width / (Content.Width * startScale);
        //            double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

        //            // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
        //            // so get the Y pixel coordinate.
        //            double renderedY = Content.Y + yOffset;
        //            double deltaY = renderedY / Height;
        //            double deltaHeight = Height / (Content.Height * startScale);
        //            double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

        //            // Calculate the transformed element pixel coordinates.
        //            double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
        //            double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

        //            // Apply translation based on the change in origin.
        //            Content.TranslationX = targetX.Clamp(-Content.Width * (currentScale - 1), 0);
        //            Content.TranslationY = targetY.Clamp(-Content.Height * (currentScale - 1), 0);

        //            // Apply scale factor.
        //            Content.Scale = currentScale;
        //        }
        //        if (e.Status == GestureStatus.Completed)
        //        {
        //            // Store the translation delta's of the wrapped user interface element.
        //            xOffset = Content.TranslationX;
        //            yOffset = Content.TranslationY;
        //        }
        //    }
    }
}