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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2048
{
    public class PossibleTextures
    {
        const int numberOfImages = 26;//2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,...

        static Texture2D[] _textures;

        public static Texture2D EmptyMatruxTexture { get; private set; }

        ContentManager _content;

        public PossibleTextures(ContentManager content)
        {
            _content = content;

            _textures = new Texture2D[numberOfImages];

            EmptyMatruxTexture = _content.Load<Texture2D>("EmptyMatrix");
        }

        public Texture2D LoadTexture(int value)
        {
            int orderInArray = orderOfTwo(value);

            if (_textures[orderInArray] == null)
            {
                _textures[orderInArray] = _content.Load<Texture2D>(GetImageName(value));
            }

            return _textures[orderInArray];
        }

        private int orderOfTwo(int value)
        {
            if ((value & (value - 1)) != 0)
            {
                throw new ArgumentException("Value of cell can be only power of 2!");
            }

            int i = 0;
            while (value / 2 != 1)
            {
                value = value/2;
                i++;
            }
            return i;
        }

        private string GetImageName(int value)
        {
            return value.ToString();
        }
    }
}