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

        static Texture2D _emptyMatrix;
        static Vector2 _positionOfCentralMatrix;
        static Vector2 _positionOfLeftMatrix;
        static Vector2 _positionOfRightMatrix;

        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        const float velocity = 1f;

        Model model;

        const int numberOfAnimationUpdates = 90;

        const double endOfAnimation = numberOfAnimationUpdates * 1.2;

        int counter = 0;

        public Rectangle LeftMatrixRectangle { get; private set; }

        public Rectangle RightMatrixRectangle { get; private set; }

        public Rectangle CentralMatrixRectangle { get; private set; }


        public FirstScreen(int cellSize,
                            Rectangle buttonSize,
                            float aspectRatio,
                            ContentManager content)
        {
            _cellSize = cellSize;

            _buttonSize = buttonSize;

            InitHardButton();

            InitEasyButton();

            InitMatrixPositions();

            _emptyMatrix = PossibleTextures.EmptyMatruxTexture;

            worldMatrix = Matrix.Identity;

            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 14),
                Vector3.Zero,
                Vector3.Up);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                aspectRatio,
                1,
                20);

            
            model = content.Load<Model>("ColoredCube");
        }

        void InitHardButton()
        {
            _hardButtonTexture = PossibleTextures.ButtonTexture;
           
            HardButton = new Rectangle(_buttonSize.X,
                                        _buttonSize.Y,
                                        _buttonSize.Width,
                                        _buttonSize.Height);
        }

        void InitEasyButton()
        {
            _easyButtonTexture = PossibleTextures.ButtonTexture;

            EasyButton = new Rectangle(_buttonSize.X,
                                        _cellSize,
                                        _buttonSize.Width,
                                        _buttonSize.Height);
        }

        public void Update()
        {
            if (counter < numberOfAnimationUpdates)
            {
                worldMatrix *= Matrix.CreateRotationX(0.05f);
                worldMatrix *= Matrix.CreateRotationY(0.05f);

            }
            counter++;
            if(counter > endOfAnimation &&
                LeftMatrixRectangle.Bottom != CentralMatrixRectangle.Top)
            {
                _positionOfRightMatrix.X += velocity;
                _positionOfRightMatrix.Y += velocity;

                RightMatrixRectangle = new Rectangle(
                    (int)_positionOfRightMatrix.X,
                    (int)_positionOfRightMatrix.Y,
                    3 * _cellSize,
                    3 * _cellSize);


                _positionOfLeftMatrix.X -= velocity;
                _positionOfLeftMatrix.Y -= velocity;

                LeftMatrixRectangle = new Rectangle(
                    (int)_positionOfLeftMatrix.X,
                    (int)_positionOfLeftMatrix.Y,
                    3 * _cellSize,
                    3 * _cellSize);
            }              
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

            
            
            if (counter < endOfAnimation)
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.TextureEnabled = true;
                        effect.EnableDefaultLighting();
                        effect.AmbientLightColor = new Vector3(0.921f, 0.484f, 0.265f);
                        effect.View = viewMatrix;
                        effect.World = worldMatrix;
                        effect.Projection = projectionMatrix;
                    }
                    mesh.Draw();
                }
            }
            else
            {
                spriteBatch.Draw(_emptyMatrix, LeftMatrixRectangle, Color.White);
                spriteBatch.DrawString(font,
                    "Back Side",
                    new Vector2(LeftMatrixRectangle.X,
                                LeftMatrixRectangle.Y + _cellSize),
                    Color.Yellow);

                spriteBatch.Draw(_emptyMatrix, CentralMatrixRectangle, Color.White);
                spriteBatch.DrawString(font,
                "Inner Side",
                new Vector2(CentralMatrixRectangle.X,
                            CentralMatrixRectangle.Y + _cellSize),
                Color.Yellow);

                spriteBatch.Draw(_emptyMatrix, RightMatrixRectangle, Color.White);
                spriteBatch.DrawString(font,
                    "Front Side",
                    new Vector2(RightMatrixRectangle.X,
                                RightMatrixRectangle.Y + _cellSize),
                    Color.Yellow);
            }
        }

        void InitMatrixPositions()
        {
            _positionOfCentralMatrix.X = GameField.CentralMatrixRectangle.X;
            _positionOfCentralMatrix.Y = GameField.CentralMatrixRectangle.Y;

            CentralMatrixRectangle = new Rectangle(
                (int)_positionOfCentralMatrix.X,
                (int)_positionOfCentralMatrix.Y,
                3 * _cellSize,
                3 * _cellSize);



            _positionOfLeftMatrix.X = _positionOfCentralMatrix.X - _cellSize;
            _positionOfLeftMatrix.Y = _positionOfCentralMatrix.Y - _cellSize;

            LeftMatrixRectangle = new Rectangle(
                (int)_positionOfLeftMatrix.X,
                (int)_positionOfLeftMatrix.Y,
                3 * _cellSize,
                3 * _cellSize);



            _positionOfRightMatrix.X = _positionOfCentralMatrix.X + _cellSize;
            _positionOfRightMatrix.Y = _positionOfCentralMatrix.Y + _cellSize;

            RightMatrixRectangle = new Rectangle(
                (int)_positionOfRightMatrix.X,
                (int)_positionOfRightMatrix.Y,
                3*_cellSize,
                3 * _cellSize);
            }
    }
}