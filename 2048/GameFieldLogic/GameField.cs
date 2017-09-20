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
using _2048.UserActions;

namespace _2048
{
    public class GameField
    {
        static Texture2D _emptyMatrix;
        static Vector2 _positionOfCentralMatrix;
        static Vector2 _positionOfLeftMatrix;
        static Vector2 _positionOfRightMatrix;

        static int _cellSize;
        static int _width;
        static int _height;
        static GraphicsDevice _graphicsDevice;

        public FieldCell[,,] FieldCells { get; set; }

        public static Rectangle LeftMatrixRectangle { get; private set; }

        public static Rectangle RightMatrixRectangle { get; private set; }

        public static Rectangle CentralMatrixRectangle { get; private set; }

        public static Rectangle UndoButton { get; private set; }

        static Texture2D _buttonTexture;

        public static Rectangle RestartButton { get; private set; }

        public GameField()
        {
            
        }
        public GameField(GameField copy)
        {
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

            InitButtons(graphicsDevice);
        }


        public void Draw(SpriteBatch spriteBatch, SpriteFont font, bool isGameOver)
        {
            spriteBatch.Draw(_emptyMatrix, CentralMatrixRectangle, Color.White);

            spriteBatch.Draw(_emptyMatrix, LeftMatrixRectangle, Color.White);

            spriteBatch.Draw(_emptyMatrix, RightMatrixRectangle, Color.White);

            spriteBatch.Draw(_buttonTexture, RestartButton, Color.White);

            int horizontalSpace = RestartButton.Width / 7;
            int verticalSpace = RestartButton.Height / 7;

            spriteBatch.DrawString(font,
                "Restart",
                new Vector2(RestartButton.X + horizontalSpace,
                            RestartButton.Y + verticalSpace),
                Color.Firebrick);

            spriteBatch.Draw(_buttonTexture, UndoButton, Color.White);

            horizontalSpace = UndoButton.Width / 3;
            verticalSpace = UndoButton.Height / 3;

            string text = "Undo";

            if (isGameOver)
            {
                text = "Try again";
            }

            Color textColor = Undo.IsMemoryEmpty ? Color.AntiqueWhite : Color.Firebrick;

            spriteBatch.DrawString(font,
                text,
                new Vector2(UndoButton.X + horizontalSpace,
                            UndoButton.Y + verticalSpace),
                textColor);
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

        void InitButtons(GraphicsDevice graphicsDevice)
        {
            _buttonTexture = PossibleTextures.ButtonTexture;

            UndoButton = new Rectangle((int)_positionOfLeftMatrix.X + _cellSize, (int)_positionOfRightMatrix.Y + 4*_cellSize, 7*_cellSize, 2*_cellSize);

            RestartButton = new Rectangle((int)_positionOfRightMatrix.X, (int)_positionOfLeftMatrix.Y - 3 * _cellSize, 3 * _cellSize, 1 * _cellSize);
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