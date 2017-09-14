using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace _2048
{
    public class CoordinatesConversion
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public int CellSize { get; set; }

        private int _gameFieldX;

        private int _gameFieldY;

        public CoordinatesConversion(int width, int height)
        {
            Width = width;
            Height = height;

            CellSize = Width/10;

            _gameFieldX = (Width - 9 * CellSize) / 2;
            _gameFieldY = (Height - 9 * CellSize) / 2;
        }

        public PixelCoordinates ToPixelCoordinates(int x, int y, int z)
        {
            var pixCoords = new PixelCoordinates();

            pixCoords.X = _gameFieldX + x * CellSize + z * 3 * CellSize;
            pixCoords.Y = _gameFieldY + y * CellSize + z * 3 * CellSize;

            return pixCoords;
        }
    }
}