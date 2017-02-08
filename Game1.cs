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

        List<TileMap> Chunks;
        TileMap previousChunk;
        TileMap currentChunk;
        FileInfo[] mapFiles;

        List<ParallaxLayer> parallaxLayers;
        Color backgroundColor;

        public static Random RandomObject;

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
            RandomObject = new Random();

            graphics.PreferredBackBufferHeight = 512;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.ApplyChanges();

            backgroundColor = new Color(86, 138, 205);

            Chunks = new List<TileMap>();

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
            mapFiles = mapDir.GetFiles();

            //TODO: Actual map generation script implementation goes here.
            //map.Generate(map.ReadImage("map_1.png"), PixelScaleFactor);
            

            //Initialize and load background parallaxing layers
            parallaxLayers = new List<ParallaxLayer>();
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_0"), new Vector2(-10, 3f), 0.0125f));
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_1"), new Vector2(-30, 3f), 0.03f));


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

            UpdateChunks();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(backgroundColor);

            //Draw background parallaxing layers with different spritebatch settings 
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            foreach (ParallaxLayer p in parallaxLayers)
                p.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            foreach(TileMap map in Chunks)
                map.Draw(spriteBatch); //Draw each active chunk
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //Maybe use this to generate chunks eventually.
        void GenerateNewChunk(Rectangle startTileRect, string mapName) {
            TileMap newChunk = new TileMap();

            Chunks.Add(newChunk);
            previousChunk = currentChunk;
            currentChunk = newChunk;

            //TODO: Generate the chunk at an origin position given by startTileRect as an "origin"
        }

        void UpdateChunks() {
            if (Chunks.Count > 0) {
                if (Chunks[0].EndTile.Rect.X <= -Chunks[0].EndTile.Rect.Width) {
                    //last tile of first chunk is off screen, destroy that chunk (i.e. remove it from the list).
                    Chunks.RemoveAt(0);
                }

                if (Chunks[Chunks.Count - 1].EndTile.Rect.X < GraphicsDevice.Viewport.Width) {
                    //End tile of last chunk is on screen, generate new chunk
                    GenerateNewChunk(Chunks[Chunks.Count - 1].EndTile.Rect, GetNewMapName());
                }
            }
        }

        //Gets a random map name from the directory of the maps
        string GetNewMapName() {
            return mapFiles[RandomObject.Next(mapFiles.Length)].Name;
        }
    }
}
