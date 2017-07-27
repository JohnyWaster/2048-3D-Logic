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

        public float X { get; set; }

        public float Y { get; set; }

        public Cell(PossibleTextures textures, int value, PixelCoordinates coords)
        {
            Value = value;

            _cellTextures = textures;

            X = coords.X;
            Y = coords.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            Color tintColor = Color.White;
            spriteBatch.Draw(_cellTextures.LoadTexture(Value), topLeftOfSprite, tintColor);
        }
    }
}