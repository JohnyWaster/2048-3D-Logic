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
    public class FirstScreen
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
            _hardButtonTexture = PossibleTextures.ButtonTexture;
           
            HardButton = new Rectangle(_buttonSize.X,
                                        _buttonSize.Y,
                                        _buttonSize.Width,
                                        _buttonSize.Height);
        }

        void InitEasyButton(GraphicsDevice graphicsDevice)
        {
            _easyButtonTexture = PossibleTextures.ButtonTexture;

            EasyButton = new Rectangle(_buttonSize.X,
                                        _cellSize,
                                        _buttonSize.Width,
                                        _buttonSize.Height);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Draw(_easyButtonTexture, EasyButton, Color.White);

            spriteBatch.Draw(_hardButtonTexture, HardButton, Color.White);

            int horizontalSpace = _buttonSize.Width/3;
            int verticalSpace = _buttonSize.Height/3;

            spriteBatch.DrawString(font,
                "Easy",
                new Vector2(EasyButton.X + horizontalSpace,
                            EasyButton.Y + verticalSpace),
                Color.Yellow);

            spriteBatch.DrawString(font,
                "Very Hard",
                new Vector2(HardButton.X + horizontalSpace,
                            HardButton.Y + verticalSpace),
                Color.Yellow);
        }
    }
}