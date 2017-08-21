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
    class GameField
    {
        Texture2D _emptyMatrix;
        Vector2 _positionOfCentralMatrix;
        Vector2 _positionOfLeftMatrix;
        Vector2 _positionOfRightMatrix;

        int _cellSize;

        public FieldCell[,,] FieldCells { get; set; }

        public Rectangle LeftMatrixRectangle { get; private set; }

        public Rectangle RightMatrixRectangle { get; private set; }

        public Rectangle CentralMatrixRectangle { get; private set; }

        public GameField(GraphicsDevice graphicsDevice, int width, int height, int cellSize)
        {
            _cellSize = cellSize;

            InitTextureOfMatrix(graphicsDevice);

            InitMatrixPositions(width, height);

            InitCellsRectangles();
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

            CentralMatrixRectangle = new Rectangle((int)_positionOfCentralMatrix.X,
                                            (int)_positionOfCentralMatrix.Y,
                                            (int)_positionOfCentralMatrix.X + 3 * _cellSize,
                                            (int)_positionOfCentralMatrix.Y + 3 * _cellSize);

            _positionOfLeftMatrix.X = _positionOfCentralMatrix.X - 3 * _cellSize;
            _positionOfLeftMatrix.Y = _positionOfCentralMatrix.Y - 3 * _cellSize;

            LeftMatrixRectangle = new Rectangle((int)_positionOfLeftMatrix.X,
                                            (int)_positionOfLeftMatrix.Y,
                                            (int)_positionOfLeftMatrix.X + 3 * _cellSize,
                                            (int)_positionOfLeftMatrix.Y + 3 * _cellSize);

            
            _positionOfRightMatrix.X = _positionOfCentralMatrix.X + 3 * _cellSize;
            _positionOfRightMatrix.Y = _positionOfCentralMatrix.Y + 3 * _cellSize;

            RightMatrixRectangle = new Rectangle((int)_positionOfRightMatrix.X,
                                            (int)_positionOfRightMatrix.Y,
                                            (int)_positionOfRightMatrix.X + 3 * _cellSize,
                                            (int)_positionOfRightMatrix.Y + 3 * _cellSize);
        }

        void InitTextureOfMatrix(GraphicsDevice graphicsDevice)
        {
            int numberOfPixelsInMatrix = 3 * 3 * _cellSize * _cellSize;

            _emptyMatrix = new Texture2D(graphicsDevice, 3 * _cellSize, 3 * _cellSize);
            Color[] colorData = new Color[numberOfPixelsInMatrix];
            for (int i = 0; i < numberOfPixelsInMatrix; i++)
                colorData[i] = Color.Coral;
            _emptyMatrix.SetData<Color>(colorData);
        }

        public void ResetEmptyCells()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        FieldCells[x, y, z].IsEmpty = true;
                    }
                }
            }
        }

        void InitCellsRectangles()
        {
            FieldCells = new FieldCell[3,3,3];

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        FieldCells[x, y, z] = new FieldCell(new Rectangle(
                            (int)_positionOfLeftMatrix.X + z * 3 * _cellSize + x * _cellSize,
                            (int)_positionOfLeftMatrix.Y + z * 3 * _cellSize + y * _cellSize,
                            _cellSize,
                            _cellSize));
                        FieldCells[x, y, z].IsEmpty = true;
                    }
                }
            }                   
        }

        public GameCoordinates? IsItGameCoordinate(Rectangle rect)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        if (rect == FieldCells[x, y, z].CellRectangle)
                        {
                            return new GameCoordinates(x, y, z);
                        }
                    }
                }
            }
            return null;
        }
    }
}