using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace The_Bond_of_Stone {

    public enum GameState { SplashScreen, MainMenu, Playing, Pause, GameOver };

	/// <summary>
	/// Main type. Handles drawing, initialization, and updates based on the State parameter, and also holds various static
	/// members that are intermittently referenced throughout the code.
	/// 
	/// By Dom Liotti, Noah Bock, and Will Dickinson
	/// </summary>
	public class Game1 : Game {
        //BASIC
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public GameState State { get; set; }

        //STATICS AND CONSTANTS
        public const int TILE_SIZE = 24;
        public const int TILE_PIXEL_SIZE = 8;
        public static int PIXEL_SCALE { get { return TILE_SIZE / TILE_PIXEL_SIZE; } }
        public static Vector2 GRAVITY = new Vector2(0, 2500f);
        public static int CHUNK_LOWER_BOUND { get { return 10 * TILE_SIZE; } }
        public static string[] DEVELOPER_NAMES = { "Dom Liotti", "Will Dickinson", "Chip Butler", "Noah Bock" };

        public static int TITAN_SPAWN_RATE = 25;

        Vector2 playerStartPos;
        Rectangle chunkStartPos;
        public float cameraSpeed = 1.5f;

        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }
        public static Random RandomObject;

        public static KeyboardState keyboardState;
        public static KeyboardState prevKeyboardState;
        public static MouseState mouseState;
        public static MouseState prevMouseState;

        //CONTENT
        Graphics LoadedGraphics;
        public static LevelGenerator Generator;
        public static Camera Camera;
        public static UI Interface;
        public static EntityManager Entities;
        public static ScoreManager Score;
        public TitanManager Titans;

        Player Player;
        public static PlayerStats PlayerStats;

        ParallaxLayer[] parallaxLayers;
        List<Entity> GlobalEntities = new List<Entity>();

        //Splash screen stuff
        public const bool SHOW_SPLASH_SCREEN = true;

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
            State = GameState.SplashScreen;

            parallaxLayers = new ParallaxLayer[2];

            playerStartPos = new Vector2(64, 64);
            chunkStartPos = new Rectangle(
                0,
                TILE_SIZE * 8,
                TILE_SIZE,
                TILE_SIZE);

            //Random object for ALL THE GAME'S RNG. Reference this Random instance ONLY
            RandomObject = new Random();

            //Adjust the screen dimensions and other particulars
            ScreenHeight = graphics.PreferredBackBufferHeight = 768;
            ScreenWidth = graphics.PreferredBackBufferWidth = 1024;
            IsMouseVisible = true;
            graphics.ApplyChanges();

			base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load all of the game's graphical content into a static object.
            LoadedGraphics = new Graphics();
            LoadedGraphics.LoadContent(Content);

            Player = new Player(Graphics.PlayerTextures[0], playerStartPos);
            PlayerStats = new PlayerStats(Player, 6);
            Interface = new UI(PlayerStats, GraphicsDevice.Viewport);
            Camera = new Camera(GraphicsDevice, Player, cameraSpeed);
            Generator = new LevelGenerator(graphics, chunkStartPos);
            Score = new ScoreManager();
            Entities = new EntityManager(Camera);
            Titans = new TitanManager(GraphicsDevice.Viewport);

            Generator.DoStarterGeneration();
            Camera.Reset();

			parallaxLayers[0] = new ParallaxLayer(Graphics.ParallaxLayers[0], Player, new Vector2(1.125f, 0f), GraphicsDevice.Viewport);
            parallaxLayers[1] = new ParallaxLayer(Graphics.ParallaxLayers[1], Player, new Vector2(2f, 0f), GraphicsDevice.Viewport);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected override void OnExiting(object sender, EventArgs args) {
            Score.RewriteFile();
            base.OnExiting(sender, args);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {

            //Update game and input states
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            switch (State) {
                case GameState.SplashScreen:
                    if (SHOW_SPLASH_SCREEN)
                        UpdateSplashScreen(gameTime);
                    else
                        State = GameState.MainMenu;

                    if (mouseState.LeftButton == ButtonState.Pressed || keyboardState.GetPressedKeys().Length > 0)
                        State = GameState.MainMenu;
                    break;
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
            prevMouseState = mouseState;

            Interface.Update(gameTime, State);

            base.Update(gameTime);
        }

        private void UpdateSplashScreen(GameTime gameTime) {
            if (Interface.DoneWithSplashScreen)
                State = GameState.MainMenu;
        }

        /// <summary>
        /// Update the game while on the main menu.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void UpdateMainMenu(GameTime gameTime) {
            if (KeyPressed(Keys.Enter))
            {
                State = GameState.Playing;
                ResetGame();
            }

            foreach (ParallaxLayer pl in parallaxLayers)
                pl.Update(gameTime);
        }

        /// <summary>
        /// Update the game with play behaviors.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void UpdatePlaying(GameTime gameTime)
        {

            Player.Update(gameTime, keyboardState, prevKeyboardState);
            PlayerStats.Update(gameTime);

            if (!PlayerStats.IsAlive)
            {
                if (PlayerStats.Health <= 0)
                    Player.KnockBack(new Vector2(-200f, -800f));
                State = GameState.GameOver;
                Camera.Target = null;
            }

            Camera.Update(gameTime);

            Titans.Update(gameTime);

            foreach (ParallaxLayer pl in parallaxLayers)
                pl.Update(gameTime);

            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape)) {
                State = GameState.Pause;
            }

            if (keyboardState.IsKeyDown(Keys.B) && prevKeyboardState.IsKeyUp(Keys.B))
            {
                Player.bounce = true;
            }

            if (keyboardState.IsKeyDown(Keys.T) && prevKeyboardState.IsKeyUp(Keys.T))
            {
                Entities.enemies.Add(new TurretEnemy(Graphics.EnemySlugTextures[4], new Vector2(Player.Position.X + 200, Player.Position.Y - 200), Projectile.Sawblade));
            }

            /*
            //Testing things
            if(keyboardState.IsKeyDown(Keys.H) && prevKeyboardState.IsKeyUp(Keys.H)) {
                PlayerStats.TakeDamage(1);
                Player.KnockBack(new Vector2(9000f, -3000f));
            }


			if (keyboardState.IsKeyDown(Keys.P) && prevKeyboardState.IsKeyUp(Keys.P)) {
				PlayerStats.TickScore();
            }

            if (keyboardState.IsKeyDown(Keys.G) && prevKeyboardState.IsKeyUp(Keys.G))
            {
                dynamicEntities.Add(new GroundEnemy(Graphics.EnemySlugTextures[0], new Vector2(Player.Position.X + 20, Player.Position.Y)));
            }

            if (keyboardState.IsKeyDown(Keys.J) && prevKeyboardState.IsKeyUp(Keys.J))
            {
                dynamicEntities.Add(new JumpingEnemy(Graphics.EnemySlugTextures[0], new Vector2(Player.Position.X + 20, Player.Position.Y)));
            }

            if (keyboardState.IsKeyDown(Keys.F) && prevKeyboardState.IsKeyUp(Keys.F))
            {
                dynamicEntities.Add(new FlyingEnemy(Graphics.EnemySlugTextures[0], new Vector2(Player.Position.X + 20, Player.Position.Y - 50)));
            }
            */

            //if (keyboardState.IsKeyDown(Keys.R) && prevKeyboardState.IsKeyUp(Keys.R)) {
            //    Camera.ScreenShake(3, 0.25f);
            //}

            Entities.Update(gameTime, State);

            //TODO: MULTITHREAD THIS LINE OPERATION WITH TASKS (?)
            Generator.UpdateChunkGeneration();

            foreach (Chunk map in Generator.Chunks)
                map.Update(gameTime); //Update each active chunk
        }

        /// <summary>
        /// Update the pause screen behaviors.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void UpdatePause(GameTime gameTime) {
            //Resume the game if the escape key is pressed again
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape)) {
                State = GameState.Playing;
            }

            if (keyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
            {
                State = GameState.MainMenu;
            }

            if (keyboardState.IsKeyDown(Keys.R) && prevKeyboardState.IsKeyUp(Keys.R))
            {
                ResetGame();
            }
        }

        /// <summary>
        /// Update the game while on the game over screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void UpdateGameOver(GameTime gameTime) {
            //TODO: IMPLEMENT GAME OVER SCREEN UPDATES
            Player.Update(gameTime, keyboardState, prevKeyboardState);
            Camera.Update(gameTime);

            foreach (ParallaxLayer pl in parallaxLayers)
                pl.Update(gameTime);
            Generator.UpdateChunkGeneration();

            foreach (Chunk map in Generator.Chunks)
                map.Update(gameTime); //Update each active chunk

            Entities.Update(gameTime, State);

            //Reset on an ESC key press.
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
                ResetGame();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Organizational: Draw according to the current game state
            switch (State) {
                case GameState.SplashScreen:
                    GraphicsDevice.Clear(Color.Black);
                    break;
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.Playing:
                    DrawPlaying(gameTime, Color.White);
                    break;
                case GameState.GameOver:
					DrawGameOver(gameTime, Color.GhostWhite);
                    break;
                case GameState.Pause:
                    DrawPause(gameTime, Color.Gray);
                    break;
            }

            //Always draw the interface. It has its own state switch.
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
            Interface.Draw(spriteBatch, gameTime, State);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draw the main menu elements.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void DrawMainMenu(GameTime gameTime) {
            //Draw the parallax layers in the background.
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointWrap);
            foreach (ParallaxLayer pl in parallaxLayers)
                pl.Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Draw the game screen elements.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void DrawPlaying(GameTime gameTime, Color color) {
            //Draw titans if necessary
            if (Titans.HasTitan) {
                spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
                Titans.Draw(spriteBatch);
                spriteBatch.End();
            }

            //Draw the parallax layers in the background.
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointWrap);
            foreach (ParallaxLayer pl in parallaxLayers)
                pl.Draw(spriteBatch);

            Camera.Draw(spriteBatch);

			spriteBatch.End();

            //Draw the foreground elements (Level, entities)
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetViewMatrix());
            foreach (Chunk map in Generator.Chunks)
                map.Draw(spriteBatch, color); //Draw each active chunk

            //Draw enemies
            Entities.Draw(spriteBatch, State);

			Player.Draw(spriteBatch, PlayerStats.invulnColor);

			spriteBatch.End();
		}

        /// <summary>
        /// Draw the paused screen (Same as the game screen elements, but with a special overlay).
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void DrawPause(GameTime gameTime, Color color) {
            DrawPlaying(gameTime, Color.White);
        }

        /// <summary>
        /// Draw the game over screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void DrawGameOver(GameTime gameTime, Color color) {
            DrawPlaying(gameTime, Color.White);
        }

        /// <summary>
        /// Resets the game.
        /// </summary>
        public void ResetGame() {
            //Generate a new level
            Generator.Restart();

            //Clear Enemies
            Entities.enemies.Clear();
            Entities.projectiles.Clear();

            Titans.Reset();

            //Reset the player and camera
            Player = new Player(Graphics.PlayerTextures[0], playerStartPos);

            Camera.Target = Player;
            Camera.Reset();

            PlayerStats.Player = Player;
            PlayerStats.Reset();

            //Reset the game state
            State = GameState.Playing;

			Interface.Reset();
        }

        public static bool KeyPressed(Keys key) {
            return (keyboardState.IsKeyDown(key) && !prevKeyboardState.IsKeyDown(key));
        }
    }
}