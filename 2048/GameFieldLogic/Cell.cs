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
using Newtonsoft.Json;

namespace _2048
{
    public class Cell
    {
        static PossibleTextures _cellTextures;

        private static decimal VELOCITY;

        private static decimal DIVIDER = 20;

        public int Value { get; set; }

        public GameCoordinates Coordinates { get; set; }

        public Rectangle OldRectangleForDiagonalMovement { get; private set; }

        public Rectangle NewRectangleForDiagonalMovement { get; private set; }

        public Rectangle CellRectangle { get; private set; }

        private PixelCoordinates _pixelCoordinates;

        public bool Active { get; set; }

        public bool ForRemove { get; set; }

        private static int _cellSize;

        private static CoordinatesConversion _conversion;

        private int _counterForDiagonalMovement = 1;

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

            VELOCITY = (decimal)_cellSize/ DIVIDER;
        }
        
        [JsonConstructor]
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

            if (IsDiagonal())
            {
                spriteBatch.Draw(
                _cellTextures.LoadTexture(Value),
                NewRectangleForDiagonalMovement,
                tintColor);

                spriteBatch.Draw(
                _cellTextures.LoadTexture(Value),
                OldRectangleForDiagonalMovement,
                tintColor);
                return;
            }
            

            spriteBatch.Draw(
                _cellTextures.LoadTexture(Value),
                CellRectangle,
                tintColor);
        }

        private bool IsDiagonal()
        {
            if (_counterForDiagonalMovement != 1)
            {
                return true;
            }
            return false;
        }

        public void Update(Vector2 direction, GameTime gameTime)
        {
            if (Active == false)
            {
                return;
            }


            //diagonal left top movement
            if (IsDiagonalLeftTop(direction))
            {
                HandleLeftTopDiagonalMovement(direction);

                CellRectangle = NewRectangleForDiagonalMovement;
                return;
            }

            //diagonal right bottom movement
            if (IsDiagonalRightBottom(direction))
            {
                HandleRightBottomDiagonalMovement(direction);

                CellRectangle = NewRectangleForDiagonalMovement;
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

        private void HandleRightBottomDiagonalMovement(Vector2 direction)
        {
            RightBottomDiagonalMovement(direction);

            OldRectangleForDiagonalMovement = new Rectangle(
                (int)_pixelCoordinates.X,
                (int)_pixelCoordinates.Y,
                (int) (_cellSize - VELOCITY*_counterForDiagonalMovement),
                (int) (_cellSize - VELOCITY*_counterForDiagonalMovement));

            NewRectangleForDiagonalMovement = new Rectangle(
                (int)_conversion.ToPixelCoordinates(Coordinates.X, Coordinates.Y, Coordinates.Z + 1).X,
                (int)_conversion.ToPixelCoordinates(Coordinates.X, Coordinates.Y, Coordinates.Z + 1).Y,
                (int) (VELOCITY*_counterForDiagonalMovement),
                (int) (VELOCITY*_counterForDiagonalMovement));

            UpdateCounterForDiagonalMovement();
        }

        private void UpdateCounterForDiagonalMovement()
        {
            _counterForDiagonalMovement++;

            if (_counterForDiagonalMovement == DIVIDER + 1)
            {
                _counterForDiagonalMovement = 1;
            }
        }

        private void LeftTopDiagonalMovement(Vector2 direction)
        {
            // if it is the beginning of diagonal movement
            if (_counterForDiagonalMovement == 1)
            {
                _pixelCoordinates.X += (decimal) direction.X*(VELOCITY + 2*_cellSize);
                _pixelCoordinates.Y += (decimal) direction.Y*(VELOCITY + 2*_cellSize);
            }
            else
            {
                _pixelCoordinates.X += (decimal) direction.X*VELOCITY;
                _pixelCoordinates.Y += (decimal) direction.Y*VELOCITY;
            }
        }

        private void RightBottomDiagonalMovement(Vector2 direction)
        {
            // if it is the finish of diagonal movement
            if (_counterForDiagonalMovement == DIVIDER)
            {
                _pixelCoordinates.X += (decimal)direction.X * (VELOCITY + 2 * _cellSize);
                _pixelCoordinates.Y += (decimal)direction.Y * (VELOCITY + 2 * _cellSize);
            }
            else
            {
                _pixelCoordinates.X += (decimal)direction.X * VELOCITY;
                _pixelCoordinates.Y += (decimal)direction.Y * VELOCITY;
            }
        }


        private void HandleLeftTopDiagonalMovement(Vector2 direction)
        {
            LeftTopDiagonalMovement(direction);

            OldRectangleForDiagonalMovement = new Rectangle(
                (int)_conversion.ToPixelCoordinates(Coordinates.X, Coordinates.Y, Coordinates.Z).X,
                (int)_conversion.ToPixelCoordinates(Coordinates.X, Coordinates.Y, Coordinates.Z).Y,
                (int)(_cellSize - VELOCITY * _counterForDiagonalMovement),
                (int)(_cellSize - VELOCITY * _counterForDiagonalMovement));

            NewRectangleForDiagonalMovement = new Rectangle(
                (int)_pixelCoordinates.X,
                (int)_pixelCoordinates.Y,
                (int)(VELOCITY * _counterForDiagonalMovement),
                (int)(VELOCITY * _counterForDiagonalMovement));

            UpdateCounterForDiagonalMovement();

        }

        private bool IsDiagonalLeftTop(Vector2 direction)
        {
            if (direction.X == -1 &&
                direction.Y == -1)
            {
                return true;
            }
            return false;
        }

        private bool IsDiagonalRightBottom(Vector2 direction)
        {
            if (direction.X == 1 &&
                direction.Y == 1)
            {
                return true;
            }
            return false;
        }
    }
}