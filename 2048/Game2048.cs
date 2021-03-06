using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Android.App;
using Android.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using _2048.DifficultyLevels;
using _2048.GameFieldLogic;
using _2048.UserActions;

namespace _2048
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game2048 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        CoordinatesConversion _conversion;


        List<Cell> _cells;
        GameField _field;
        int _widthScreen;
        int _heightScreen;

        SpriteFont _font;

        Score _score = new Score();

        public Score myScore
        {
            get { return _score; }
        }

        IMoveFinisher _moveFinisher;

        InputHandler _inputHandler;

        PossibleTextures _textures;

        CellsCombiner _cellsCombiner;

        FirstScreen _firstScreen;

        Undo _undo;

        public FirstScreen FirstScreen
        {
            get { return _firstScreen; }
        }

        bool _isGameOver;

        public Game2048()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsFixedTimeStep = false;
            this.graphics.SynchronizeWithVerticalRetrace = false;

            InputHandler.AddRestartHandler(RestartGame);
            InputHandler.AddUndoHandler(UndoButtonPressed);

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _widthScreen = GraphicsDevice.Viewport.Width;
            _heightScreen = GraphicsDevice.Viewport.Height;

            _textures = new PossibleTextures(Content);

            _conversion = new CoordinatesConversion(_widthScreen, _heightScreen);

            _font = GetFont(_conversion.CellSize);

            _field = new GameField(GraphicsDevice, _widthScreen, _heightScreen, _conversion.CellSize);

            _cells = new List<Cell>();

            _undo = new Undo();

            _firstScreen = new FirstScreen(_conversion.CellSize,
                GameField.UndoButton,
                graphics.GraphicsDevice.Viewport.AspectRatio,
                Content);

            AddCell(new GameTime(TimeSpan.Zero, TimeSpan.Zero));

        
            var savedState = State.LoadGameState();

            if (savedState != null)
            {
                RestoreSavedState(savedState);
            }
         

            _inputHandler = new InputHandler(_cells, _field);

            _cellsCombiner = new CellsCombiner(_field, _cells, _score);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            //wait user input, nothing to update
            if (_moveFinisher == null)
            {
                //difficuly level feature
                if (_firstScreen.DifficultyLevel == null)
                {
                    _firstScreen.Update();
                    _firstScreen.DifficultyLevel = _inputHandler.GetDifficultyLevel(_firstScreen);

                    if (_firstScreen.DifficultyLevel != null)
                    {
                        _score.LoadBestScore(_firstScreen.DifficultyLevel);
                    }
                    return;
                }



                //game over feature
                if (_isGameOver)
                {
                    if (_inputHandler.TryAgain())
                    {
                        RestartGame();
                    }
                    else
                    {
                        return;
                    }
                }

                _moveFinisher = _inputHandler.GetUserAction();

                if (_moveFinisher != null)
                {
                    Undo.SaveState(_field, _cells, _score);

                    foreach (var cell in _cells)
                    {
                        cell.Active = true;
                    }
                }
            }

            //game process
            if (_moveFinisher != null)
            {
                //all this tricks with multiple times calls methods
                //of _moveFinisher because of _cells is unordered list
                //so we must be sure that update correctly all cells.
                _moveFinisher.DeactivateFinishedCells();
                _moveFinisher.DeactivateFinishedCells();

                //two times for solving next issue: 2,2,4 -> _,_,8
                _cellsCombiner.CombineCells(_moveFinisher.Direction);
                _cellsCombiner.CombineCells(_moveFinisher.Direction);

                _moveFinisher.ActivateFalselyFinishedCells();
                _moveFinisher.DeactivateFinishedCells();


                int unFinishedCellsCounter = 0;

                foreach (var cell in _cells)
                {
                    if (cell.Active)
                    {
                        cell.Update(_moveFinisher.Direction, gameTime);

                        var newCoordinates = _field.IsItGameCoordinate(cell.CellRectangle);

                        if (newCoordinates != null)
                        {
                            cell.Coordinates = (GameCoordinates)newCoordinates;
                        }

                        unFinishedCellsCounter++;
                    }
                }

                if (unFinishedCellsCounter == 0)
                {
                    _moveFinisher = null;
                    AddCell(gameTime);
                    _field.ResetEmptyCells();
                }
            }

            base.Update(gameTime);
        }




        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (_firstScreen.DifficultyLevel == null)
            {
                _firstScreen.Draw(spriteBatch, _font);
                spriteBatch.End();

                return;
            }

            _field.Draw(spriteBatch, _font, _isGameOver);

            foreach (var cell in _cells)
            {
                cell.Draw(spriteBatch);
            }

            spriteBatch.DrawString(_font, "Score: " + _score.ScoreValue,
                new Vector2(GameField.LeftMatrixRectangle.X,
                    _conversion.CellSize / 2),
                Color.Firebrick);

            spriteBatch.DrawString(_font, "Best: " + _score.BestScore,
                new Vector2(GameField.LeftMatrixRectangle.X,
                    3 * _conversion.CellSize / 2),
                Color.Firebrick);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void AddCell(GameTime gameTime)
        {
            _isGameOver = _field.LackOfEmptyCells();
            if (_isGameOver)
            {
                return;
            }

            var rand = new Random((int)gameTime.ElapsedGameTime.Ticks);

            if (_firstScreen.DifficultyLevel == DifficultyLevel.VeryHard)
            {
                AddThreeCells(rand);
                return;
            }

            var coords = new GameCoordinates(rand.Next(0, 3), rand.Next(0, 3), rand.Next(0, 3));

            while (!_field.FieldCells[coords.X, coords.Y, coords.Z].IsEmpty)
            {
                coords = new GameCoordinates(rand.Next(0, 3), rand.Next(0, 3), rand.Next(0, 3));
            }

            AddCellWithKnownCoordinates(coords, rand);
        }

        private void AddThreeCells(Random rand)
        {
            for (int z = 0; z < 3; z++)
            {
                AddCellInPartucularMatrix(z, rand);
            }
        }

        private void AddCellInPartucularMatrix(int zCoordinate, Random rand)
        {
            if (_field.LackOfEmptyCellsInParticularMatrix(zCoordinate))
            {
                return;
            }

            var coords = new GameCoordinates(rand.Next(0, 3), rand.Next(0, 3), zCoordinate);

            while (!_field.FieldCells[coords.X, coords.Y, coords.Z].IsEmpty)
            {
                coords = new GameCoordinates(rand.Next(0, 3), rand.Next(0, 3), zCoordinate);
            }

            AddCellWithKnownCoordinates(coords, rand);
        }

        private void AddCellWithKnownCoordinates(GameCoordinates coords, Random rand)
        {
            // 0.25 probability of 4 and 0.75 of 2
            int value = rand.Next(0, 4) == 0 ? 4 : 2;

            var cell = new Cell(_textures, value, coords, _conversion);
            _cells.Add(cell);
        }

        public State GetGameState()
        {
            Undo.CleanMemory();
            return new State(_field, _cells, _score, FirstScreen.DifficultyLevel);
        }

        private void RestoreSavedState(State savedState)
        {
            _field = savedState.Field;
            _cells = savedState.Cells;
            _score = savedState.Score;
            _firstScreen.DifficultyLevel = savedState.DifficultyLevel;           
        }

        private void RestartGame()
        {
            State.DeleteSavedGame();
            _score.SaveBestScore(_firstScreen.DifficultyLevel);
            _score.ScoreValue = 0;
            Initialize();
            Undo.CleanMemory();           
        }

        private void UndoButtonPressed()
        {
            _undo.RestoreState(ref _field, ref _cells, ref _score);

            _moveFinisher = null;

            _inputHandler = new InputHandler(_cells, _field);

            _cellsCombiner = new CellsCombiner(_field, _cells, _score);
        }

        private SpriteFont GetFont(int cellSize)
        {
            string prefix = "Arial";

            int postfix = 24;

            SpriteFont font = Content.Load<SpriteFont>(prefix + (postfix).ToString());

            int correctSize = postfix;

            //fonts between 24 and 72 with step 4 are loaded in project
            while (font.MeasureString("Inner Side").X < cellSize*3 && postfix <= 68)
            {
                correctSize = postfix;
                postfix += 4;
                font = Content.Load<SpriteFont>(prefix + (postfix).ToString());
            }
            
            return Content.Load<SpriteFont>(prefix + (correctSize).ToString());
        }
    }
}
