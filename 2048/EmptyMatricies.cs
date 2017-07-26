using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Service.Voice;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2048
{
    class EmptyMatricies
    {
        Texture2D _emptyMatrix;
        Vector2 _positionOfCentralMatrix;
        Vector2 _positionOfLeftMatrix;
        Vector2 _positionOfRightMatrix;

        int _cellSize = 80;
        
        public EmptyMatricies(GraphicsDevice graphicsDevice, float width, float height)
        {
            InitTextureOfMatrix(graphicsDevice);

            InitMatrixPositions(width, height);
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_emptyMatrix, _positionOfCentralMatrix);

            spriteBatch.Draw(_emptyMatrix, _positionOfLeftMatrix);

            spriteBatch.Draw(_emptyMatrix, _positionOfRightMatrix);
        }

        void InitMatrixPositions(float width, float height)
        {
            _positionOfCentralMatrix.X = (width - 3 * _cellSize) / 2;
            _positionOfCentralMatrix.Y = (height - 3 * _cellSize) / 2;

            _positionOfLeftMatrix.X = _positionOfCentralMatrix.X - 3 * _cellSize;
            _positionOfLeftMatrix.Y = _positionOfCentralMatrix.Y - 3 * _cellSize;

            _positionOfRightMatrix.X = _positionOfCentralMatrix.X + 3 * _cellSize;
            _positionOfRightMatrix.Y = _positionOfCentralMatrix.Y + 3 * _cellSize;
        }

        void InitTextureOfMatrix(GraphicsDevice graphicsDevice)
        {
            int numberOfPixelsInMatrix = 3 * 3 * _cellSize * _cellSize;

            _emptyMatrix = new Texture2D(graphicsDevice, 240, 240);
            Color[] colorData = new Color[numberOfPixelsInMatrix];
            for (int i = 0; i < numberOfPixelsInMatrix; i++)
                colorData[i] = Color.Coral;
            _emptyMatrix.SetData<Color>(colorData);
        }
    }
}