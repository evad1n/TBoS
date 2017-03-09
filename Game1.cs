using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.IO;
using MonoGame.Extended;
using FarseerPhysics.Dynamics;

namespace TheBondOfStone {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 

    public enum GameState { MainMenu, Playing, Pause, GameOver };

    public class Game1 : Game {
        //The game's camera
        Camera Camera { get; set; }

        public GameState state { get; set; }
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int PixelScaleFactor { get; set; }
		public static int UIScaleFactor { get; set; }
        public static int screenWidth { get; set; }
        public static int screenHeight { get; set; }

        public static Texture2D[] foregroundTiles { get; set; }
        public static Texture2D[] backgroundTiles { get; set; }

        public static Texture2D emptyTile { get; set; }

        LevelGenerator Generator { get; set; }
		UI UIManager { get; set; }

        List<ParallaxLayer> parallaxLayers;
        Color backgroundColor;

        public static Random RandomObject;

        public Player player;
        Texture2D playerTexture;

        bool reset;

        public static KeyboardState keyboardState;
        public static KeyboardState prevKeyboardState;

        public static World world;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            //Scaling factor for ALL of the game's sprites
            PixelScaleFactor = 24;
			UIScaleFactor = PixelScaleFactor / 7;

            //Set initial game state
            state = GameState.Playing;
            reset = false;

            //Random object for ALL THE GAME'S RNG. Reference this Random instance ONLY
            RandomObject = new Random();

            //Adjust the screen dimensions and other particulars
            screenHeight = graphics.PreferredBackBufferHeight = 512;
            screenWidth = graphics.PreferredBackBufferWidth = 1024;
            graphics.ApplyChanges();

            backgroundColor = new Color(86, 138, 205);

            foregroundTiles = new Texture2D[16];
            backgroundTiles = new Texture2D[16];

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Set the static Tile and TileDecoration classes to reference the Game's content loader, so they can load their own textures as needed.
            Tile.Content = Content;
            TileDecoration.Content = Content;
            world = new World(new Vector2(0, 50.0f));

            playerTexture = Content.Load<Texture2D>(@"graphics\entity\player");

            //Load tilesets
            for (int i = 0; i < 16; i++)
            {
                foregroundTiles[i] = Content.Load<Texture2D>(@"graphics\tile\tile_1_" + i);
                backgroundTiles[i] = Content.Load<Texture2D>(@"graphics\tile\tile_2_" + i);
            }
            emptyTile = Content.Load<Texture2D>("tile_0");

            SpawnPlayer();

			UIManager = new UI(this, player.p);
			UIManager.LoadContent(Content);

			//Instantiate the camera object
			Camera = new Camera(GraphicsDevice);

            //Instantiate the level generator
            Generator = new LevelGenerator(Camera);

            //Use the generator to generate the starter chunk
            Generator.DoStarterGeneration();

            //Initialize and load background parallaxing layers
            parallaxLayers = new List<ParallaxLayer>();
            //Foreground Cloud
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_0"), new Vector2(10, .5f), new Vector2(-0.0125f, 0f), GraphicsDevice.Viewport));
            //Foreground particles
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_2"), new Vector2(30, 4f), new Vector2(-0.5f, 0.125f), GraphicsDevice.Viewport));
            //Background Cloud
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_1"), new Vector2(30, .6f), new Vector2(-0.03f, 0f), GraphicsDevice.Viewport));
            //Background particles
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_3"), new Vector2(10, 3f), new Vector2(-0.1f, 0.25f), GraphicsDevice.Viewport));

            SpawnPlayer();
            Camera.target = player;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {

            //Update game and input states
            keyboardState = Keyboard.GetState();

            switch (state) {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.Playing:
                    UpdatePlaying(gameTime);
                    break;
                case GameState.GameOver:
                    UpdateGameOver(gameTime);
                    break;
                case GameState.Pause:
                    UpdatePause(gameTime);
                    break;
            }

            prevKeyboardState = keyboardState;

            base.Update(gameTime);
        }

        /// <summary>
        /// Update the game while on the main menu.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void UpdateMainMenu(GameTime gameTime) {
            //TODO: IMPLEMENT MAIN MENU UPDATES
        }

        /// <summary>
        /// Update the game with play behaviors.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void UpdatePlaying(GameTime gameTime) {
            //Physics garbage
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            //TODO: MULTITHREAD THIS LINE OPERATION WITH TASKS
            Generator.UpdateChunkGeneration();

            //Update the Camera and the player object
            player.Update(gameTime, world);
            Camera.Update(gameTime);

            if(!player.Alive)
            {
                Camera.ScreenShake(5, 2f, true);
                backgroundColor = Color.Gray;
                state = GameState.GameOver;
                Camera.target = null;
            }

            //Update the parallaxed layers
            foreach(ParallaxLayer pl in parallaxLayers)
                pl.Update(gameTime);


            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape)) {
                state = GameState.Pause;
                backgroundColor = new Color(0,119,190); //Darker CornflowerBlue
            }

            //TESTING
            if (keyboardState.IsKeyDown(Keys.Q)) {
                Camera.ScreenShake(5, 1f, false);
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                Camera.ScreenShake(5, 1f, true);
            }
        }

        /// <summary>
        /// Update the pause screen behaviors.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void UpdatePause(GameTime gameTime) {
            //Resume the game if the escape key is pressed again
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape)) {
                state = GameState.Playing;
                backgroundColor = Color.CornflowerBlue;
            }
        }

        /// <summary>
        /// Update the game while on the game over screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void UpdateGameOver(GameTime gameTime) {
            //TODO: IMPLEMENT GAME OVER SCREEN UPDATES
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))  //This causes an exception to be thrown.
            {
                if(!reset)
                {
                    Reset();
                }
                backgroundColor = Color.CornflowerBlue;
                state = GameState.Playing;
                reset = false;
            }

            Generator.UpdateChunkGeneration();
            Camera.Update(gameTime);
            foreach (ParallaxLayer pl in parallaxLayers)
                pl.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(backgroundColor);

            switch (state)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.Playing:
                    DrawPlaying(gameTime, Color.White);
                    break;
                case GameState.GameOver:
                    DrawGameOver(gameTime, Color.Red);
                    break;
                case GameState.Pause:
                    DrawPause(gameTime, Color.Gray);
                    break;
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Draw the main menu elements.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void DrawMainMenu(GameTime gameTime) {
            //TODO: IMPLEMENT MAIN MENU
        }

        /// <summary>
        /// Draw the game screen elements.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void DrawPlaying(GameTime gameTime, Color color)
        {
            //Draw background parallaxing layers with different spritebatch settings 
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointWrap);
            foreach (ParallaxLayer p in parallaxLayers)
                if (p != parallaxLayers[3])
                    p.Draw(spriteBatch, color);

            spriteBatch.End();

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetViewMatrix());
            foreach (Chunk map in Generator.Chunks)
                map.Draw(spriteBatch, color); //Draw each active chunk

            player.Draw(spriteBatch, color);
            Camera.Draw(spriteBatch, color);
            spriteBatch.End();

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointWrap);
            parallaxLayers[3].Draw(spriteBatch, color);
			UIManager.Draw(spriteBatch);
			spriteBatch.End();

        }

        /// <summary>
        /// Draw the paused screen (Same as the game screen elements, but with a special overlay).
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void DrawPause(GameTime gameTime, Color color) {
            DrawPlaying(gameTime, color);

            //TODO: IMPLEMENT OTHER PAUSED SCREEN DRAWS
        }

        /// <summary>
        /// Draw the game over screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void DrawGameOver(GameTime gameTime, Color color) {
            DrawPlaying(gameTime, color);

            //TODO: DISPLAY GAMEOVER.
        }

        void SpawnPlayer() {
            player = new Player(world, playerTexture, new Vector2(PixelScaleFactor, PixelScaleFactor), 10f, UnitConvert.ToWorld(new Vector2(86 * PixelScaleFactor, 20 * PixelScaleFactor)), Camera);
        }

        private void Reset()
        {
            Console.WriteLine("RESET");
            Camera.Reset();
            Generator.Restart();
            SpawnPlayer();
            Camera.target = player;
            player.p.Reset();
            reset = true;
        }
    }
}
