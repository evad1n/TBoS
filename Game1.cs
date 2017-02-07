using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace TheBondOfStone {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int PixelScaleFactor { get; set; }

        TileMap map;
        int mapCount;

        List<ParallaxLayer> parallaxLayers;

        Random randomObject = new Random();

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
            PixelScaleFactor = 16;

            map = new TileMap(randomObject);

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

            //Get the count of maps in the maps folder
            string path = Directory.GetCurrentDirectory();
            DirectoryInfo mapDir = new DirectoryInfo(Path.GetFullPath(Path.Combine(path, @"..\..\..\..\Content\maps"))); //THIS PATH MAY NEED TO BE AMENDED IN THE FUTURE 
            mapCount = mapDir.GetFiles().Length;
            
            //TODO: Actual map generation script implementation goes here.
            map.Generate(map.ReadImage("map_1.png"), PixelScaleFactor);

            //Initialize and load background parallaxing layers
            parallaxLayers = new List<ParallaxLayer>();
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_0"), new Vector2(-25, 1f)));
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_1"), new Vector2(-50, 2f)));


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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //DEBUG/TESTING MOVEMENT. UPDATE WITH CHARACTER/CAMERA UPDATE CALLS
            KeyboardState kbState = Keyboard.GetState();

            //Get directional vector based on keyboard input
            Vector2 direction = Vector2.Zero;
            if (kbState.IsKeyDown(Keys.Up))
                direction = new Vector2(0, -1);
            else if (kbState.IsKeyDown(Keys.Down))
                direction = new Vector2(0, 1);
            if (kbState.IsKeyDown(Keys.Left))
                direction += new Vector2(-1, 0);
            else if (kbState.IsKeyDown(Keys.Right))
                direction += new Vector2(1, 0);

            //Update backgrounds
            foreach (ParallaxLayer p in parallaxLayers)
                p.Update(gameTime, direction, GraphicsDevice.Viewport);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw background parallaxing layers with different spritebatch settings 
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            foreach (ParallaxLayer p in parallaxLayers)
                p.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            map.Draw(spriteBatch); //Draw the map (will be a list of maps in the future)
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //Maybe use this to generate chunks eventually.
        void GenerateNewChunk() {

        }
    }
}
