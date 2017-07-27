using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2048
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        CoordinatesConversion conversion;

        List<Cell> _cells;
        EmptyMatricies _field;
        int _widthScreen;
        int _heightScreen;

        double counter = 0;

        PossibleTextures _textures;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsFixedTimeStep = false;
            this.graphics.SynchronizeWithVerticalRetrace = false;


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

            conversion = new CoordinatesConversion(_widthScreen, _heightScreen);

            _field = new EmptyMatricies(GraphicsDevice, _widthScreen, _heightScreen, conversion.CellSize);

            _cells = new List<Cell>();

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
            // TODO: Unload any non ContentManager content here
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

            counter++;
            // TODO: Add your update logic here
            if (counter % 100 == 0)
            {
                var rand = new Random((int)gameTime.ElapsedGameTime.Ticks);

                var coords = conversion.ToPixelCoordinates(rand.Next(0, 3), rand.Next(0, 3), rand.Next(0, 3));

                var cell = new Cell(_textures, (int) Math.Pow(2, rand.Next(1, 14)), coords);
                _cells.Add(cell);
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

            _field.Draw(spriteBatch);

            foreach (var cell in _cells)
            {
                cell.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
