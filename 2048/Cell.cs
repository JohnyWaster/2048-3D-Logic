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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2048
{
    class Cell
    {
        static PossibleTextures _cellTextures;

        public int Value { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        private int _cellSize;

        public Cell(PossibleTextures textures, int value, PixelCoordinates coords, int cellSize)
        {
            Value = value;

            _cellTextures = textures;

            _cellSize = cellSize;

            X = coords.X;
            Y = coords.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color tintColor = Color.White;
            Rectangle distinctionRectangle = new Rectangle(X, Y, _cellSize, _cellSize);
            spriteBatch.Draw(_cellTextures.LoadTexture(Value), distinctionRectangle, tintColor);
        }
    }
}