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

        private const float VELOCITY = 10f;

        public int Value { get; set; }

        public GameCoordinates Coordinates { get; set; }

        
        public Rectangle CellRectangle { get; private set; }

        private PixelCoordinates _pixelCoordinates;

        public bool Active { get; set; }

        private int _cellSize;

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
                _pixelCoordinates.X,
                _pixelCoordinates.Y,
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

            _pixelCoordinates.X += (int) (direction.X);
            _pixelCoordinates.Y += (int)(direction.Y);

            CellRectangle = new Rectangle(
    _pixelCoordinates.X,
    _pixelCoordinates.Y,
    _cellSize,
    _cellSize);


        }
    }
}