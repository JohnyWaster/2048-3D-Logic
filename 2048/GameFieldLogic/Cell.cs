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

        private static decimal VELOCITY;

        public int Value { get; set; }

        public GameCoordinates Coordinates { get; set; }

        
        public Rectangle CellRectangle { get; private set; }

        private PixelCoordinates _pixelCoordinates;

        public bool Active { get; set; }

        public bool ForRemove { get; set; }

        private static int _cellSize;

        private static CoordinatesConversion _conversion;

        public Cell(PossibleTextures textures, int value, GameCoordinates coords, CoordinatesConversion conversion)
        {
            Value = value;

            _cellTextures = textures;

            _conversion = conversion;

            _cellSize = _conversion.CellSize;

            Coordinates = coords;

            _pixelCoordinates = _conversion.ToPixelCoordinates(
                Coordinates.X,
                Coordinates.Y,
                Coordinates.Z);

            CellRectangle = new Rectangle(
                (int)_pixelCoordinates.X,
                (int)_pixelCoordinates.Y,
                _cellSize,
                _cellSize);

            Active = true;

            ForRemove = false;

            VELOCITY = (decimal)_cellSize/20;
        }
        
        public Cell(int value, GameCoordinates coords)
        {
            Value = value;

            Coordinates = coords;

            _pixelCoordinates = _conversion.ToPixelCoordinates(
                Coordinates.X,
                Coordinates.Y,
                Coordinates.Z);

            CellRectangle = new Rectangle(
                (int)_pixelCoordinates.X,
                (int)_pixelCoordinates.Y,
                _cellSize,
                _cellSize);

            Active = true;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            Color tintColor = Color.White;


            spriteBatch.Draw(
                _cellTextures.LoadTexture(Value),
                CellRectangle,
                tintColor);
        }

        public void Update(Vector2 direction, GameTime gameTime)
        {
            if (Active == false)
            {
                return;
            }

            _pixelCoordinates.X += (decimal)direction.X * VELOCITY;
            _pixelCoordinates.Y += (decimal)direction.Y * VELOCITY;

            CellRectangle = new Rectangle(
    (int)_pixelCoordinates.X,
    (int)_pixelCoordinates.Y,
    _cellSize,
    _cellSize);


        }
    }
}