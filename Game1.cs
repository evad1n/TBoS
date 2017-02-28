using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        Camera camera;
        //The speed of the camera
        Vector2 cameravelocity;

        GameState state;
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int PixelScaleFactor { get; set; }

        LevelGenerator Generator { get; set; }

        List<ParallaxLayer> parallaxLayers;
        Color backgroundColor;

        public static Random RandomObject;

        public Player player;
        Texture2D playerTexture;

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
            PixelScaleFactor = 16;

            //Set initial game state
            state = GameState.Playing;

            //Random object for ALL THE GAME'S RNG. Reference this Random instance ONLY
            RandomObject = new Random();

            //Adjust the screen dimensions and other particulars
            graphics.PreferredBackBufferHeight = 512;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.ApplyChanges();

            backgroundColor = new Color(86, 138, 205);

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

            //TODO: Actual map generation script implementation goes here.

            

            playerTexture = Content.Load<Texture2D>(@"graphics\entity\player");

            SpawnPlayer();

            //Instantiate the camera object
            camera = new Camera(GraphicsDevice, player);

            //Instantiate the level generator
            Generator = new LevelGenerator(camera);

            //Use the generator to generate the starter chunk
            Generator.DoStarterGeneration();

            //Initialize and load background parallaxing layers
            parallaxLayers = new List<ParallaxLayer>();
            //Foreground Cloud
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_0"), new Vector2(10, .5f), new Vector2(-0.0125f, 0f)));
            //Foreground particles
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_2"), new Vector2(30, 4f), new Vector2(-0.5f, 0.125f)));
            //Background Cloud
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_1"), new Vector2(30, .6f), new Vector2(-0.03f, 0f)));
            //Background particles
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_3"), new Vector2(10, 3f), new Vector2(-0.1f, 0.25f)));
            

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

            keyboardState = Keyboard.GetState();

            switch (state)
            {
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

        void UpdateMainMenu(GameTime gameTime)
        {

        }

        void UpdatePlaying(GameTime gameTime)
        {

            //DEBUG/TESTING MOVEMENT. UPDATE WITH CHARACTER/CAMERA UPDATE CALLS
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            Generator.UpdateChunkGeneration();

            //Update the keyboard states and the player object

            player.Update(gameTime, world);


            if(keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
            {
                state = GameState.Pause;
            }
        }

        void UpdatePause(GameTime gameTime)
        {
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
            {
                state = GameState.Playing;
            }
        }

        void UpdateGameOver(GameTime gameTime)
        {

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
                    DrawPlaying(gameTime);
                    break;
                case GameState.GameOver:
                    DrawGameOver(gameTime);
                    break;
                case GameState.Pause:
                    DrawPause(gameTime);
                    break;
            }
            base.Draw(gameTime);
        }

        void DrawMainMenu(GameTime gameTime)
        {

        }

        void DrawPlaying(GameTime gameTime)
        {
            //Draw background parallaxing layers with different spritebatch settings 
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            foreach (ParallaxLayer p in parallaxLayers)
                if (p != parallaxLayers[3])
                    p.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix());
            foreach (Chunk map in Generator.Chunks)
                map.Draw(spriteBatch); //Draw each active chunk

            player.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            parallaxLayers[3].Draw(spriteBatch);
            spriteBatch.End();
        }

        void DrawPause(GameTime gameTime)
        {

        }

        void DrawGameOver(GameTime gameTime)
        {

        }

        void SpawnPlayer()
        {
            player = new Player(world, playerTexture, new Vector2(PixelScaleFactor, PixelScaleFactor), 10f, UnitConvert.ToWorld(new Vector2(24 * PixelScaleFactor, 20 * PixelScaleFactor)));
        }
    }
}
