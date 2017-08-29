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
        static Texture2D _emptyMatrix;
        static Vector2 _positionOfCentralMatrix;
        static Vector2 _positionOfLeftMatrix;
        static Vector2 _positionOfRightMatrix;

        int _cellSize;
        int _width;
        int _height;
        GraphicsDevice _graphicsDevice;

        public FieldCell[,,] FieldCells { get; set; }

        public static Rectangle LeftMatrixRectangle { get; private set; }

        public static Rectangle RightMatrixRectangle { get; private set; }

        public static Rectangle CentralMatrixRectangle { get; private set; }

        public static Rectangle UndoButton { get; private set; }

        static Texture2D _buttonTexture;

        public GameField(GameField copy)
        {
            _cellSize = copy._cellSize;
            _width = copy._width;
            _height = copy._height;
            _graphicsDevice = copy._graphicsDevice;

            FieldCells = copy.FieldCells;
        }

        public GameField(GraphicsDevice graphicsDevice, int width, int height, int cellSize)
        {
            _cellSize = cellSize;
            _width = width;
            _height = height;
            _graphicsDevice = graphicsDevice;

            InitTextureOfMatrix(graphicsDevice);

            InitMatrixPositions(width, height);

            InitCellsRectangles();

            InitUndoButton(graphicsDevice);
        }


        public void Draw(SpriteBatch spriteBatch, SpriteFont font, bool isGameOver)
        {
            spriteBatch.Draw(_emptyMatrix, CentralMatrixRectangle, Color.White);

            spriteBatch.Draw(_emptyMatrix, LeftMatrixRectangle, Color.White);

            spriteBatch.Draw(_emptyMatrix, RightMatrixRectangle, Color.White);

            spriteBatch.Draw(_buttonTexture, new Vector2(UndoButton.X, UndoButton.Y));

            int horizontalSpace = UndoButton.Width / 3;
            int verticalSpace = UndoButton.Height / 3;

            string text = "Undo";

            if (isGameOver)
            {
                text = "Try again";
            }

            spriteBatch.DrawString(font,
                text,
                new Vector2(UndoButton.X + horizontalSpace,
                            UndoButton.Y + verticalSpace),
                Color.Firebrick );
        }

        void InitMatrixPositions(float width, float height)
        {
            _positionOfCentralMatrix.X = (width - 3 * _cellSize) / 2;
            _positionOfCentralMatrix.Y = (height - 3 * _cellSize) / 2;

            CentralMatrixRectangle = new Rectangle((int)_positionOfCentralMatrix.X,
                                            (int)_positionOfCentralMatrix.Y,
                                            3 * _cellSize,
                                            3 * _cellSize);

            _positionOfLeftMatrix.X = _positionOfCentralMatrix.X - 3 * _cellSize;
            _positionOfLeftMatrix.Y = _positionOfCentralMatrix.Y - 3 * _cellSize;

            LeftMatrixRectangle = new Rectangle((int)_positionOfLeftMatrix.X,
                                            (int)_positionOfLeftMatrix.Y,
                                            3 * _cellSize,
                                            3 * _cellSize);

            
            _positionOfRightMatrix.X = _positionOfCentralMatrix.X + 3 * _cellSize;
            _positionOfRightMatrix.Y = _positionOfCentralMatrix.Y + 3 * _cellSize;

            RightMatrixRectangle = new Rectangle((int)_positionOfRightMatrix.X,
                                            (int)_positionOfRightMatrix.Y,
                                            3 * _cellSize,
                                            3 * _cellSize);
        }

        void InitTextureOfMatrix(GraphicsDevice graphicsDevice)
        {
            _emptyMatrix = PossibleTextures.EmptyMatruxTexture;
        }

        void InitUndoButton(GraphicsDevice graphicsDevice)
        {
            int numberOfPixelsInButton =  7 * 2 * _cellSize * _cellSize;

            _buttonTexture = new Texture2D(graphicsDevice, 7*_cellSize, 2*_cellSize);

            Color[] colorData = new Color[numberOfPixelsInButton];
            for (int i = 0; i < numberOfPixelsInButton; i++)
            {
                colorData[i] = Color.BurlyWood;
            }

            _buttonTexture.SetData<Color>(colorData);

            UndoButton = new Rectangle((int)_positionOfLeftMatrix.X + _cellSize, (int)_positionOfRightMatrix.Y + 4*_cellSize, 7*_cellSize, 2*_cellSize);
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

        public bool LackOfEmptyCells()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        if (FieldCells[x, y, z].IsEmpty)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        public bool LackOfEmptyCellsInParticularMatrix(int zCoordinate)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (FieldCells[x, y, zCoordinate].IsEmpty)
                    {
                        return false;
                    }
                }
            }
            return true;
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