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
using Microsoft.Xna.Framework.Graphics;

namespace _2048.DifficultyLevels
{
    class FirstScreen
    {
        public Rectangle EasyButton { get; private set; }

        Texture2D _easyButtonTexture;

        public Rectangle HardButton { get; private set; }

        Texture2D _hardButtonTexture;

        int _cellSize;

        public DifficultyLevel? DifficultyLevel;

        Rectangle _buttonSize;
  

        public FirstScreen(GraphicsDevice graphicsDevice,
                            int cellSize,
                            Rectangle buttonSize)
        {
            _cellSize = cellSize;

            _buttonSize = buttonSize;

            InitHardButton(graphicsDevice);

            InitEasyButton(graphicsDevice);
        }

        void InitHardButton(GraphicsDevice graphicsDevice)
        {
            int numberOfPixelsInButton = _buttonSize.Height * _buttonSize.Width;

            _hardButtonTexture = new Texture2D(graphicsDevice, _buttonSize.Width, _buttonSize.Height);

            Color[] colorData = new Color[numberOfPixelsInButton];
            for (int i = 0; i < numberOfPixelsInButton; i++)
            {
                colorData[i] = Color.BurlyWood;
            }

            _hardButtonTexture.SetData<Color>(colorData);

            HardButton = new Rectangle(_buttonSize.X,
                                        _buttonSize.Y,
                                        _buttonSize.Width,
                                        _buttonSize.Height);
        }

        void InitEasyButton(GraphicsDevice graphicsDevice)
        {
            int numberOfPixelsInButton = _buttonSize.Height * _buttonSize.Width;

            _easyButtonTexture = new Texture2D(graphicsDevice, _buttonSize.Width, _buttonSize.Height);

            Color[] colorData = new Color[numberOfPixelsInButton];
            for (int i = 0; i < numberOfPixelsInButton; i++)
            {
                colorData[i] = Color.BurlyWood;
            }

            _easyButtonTexture.SetData<Color>(colorData);

            EasyButton = new Rectangle(_buttonSize.X,
                                        _cellSize,
                                        _buttonSize.Width,
                                        _buttonSize.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_easyButtonTexture, new Vector2(EasyButton.X, EasyButton.Y));

            spriteBatch.Draw(_hardButtonTexture, new Vector2(HardButton.X, HardButton.Y));
        }
    }
}