﻿using Microsoft.Xna.Framework;
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
    public class Game1 : Game {
        //The game's camera
        Camera2D camera;
        //The speed of the camera
        Vector2 cameravelocity;
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int PixelScaleFactor { get; set; }

        List<TileMap> Chunks;
        FileInfo[] mapFiles;

        List<ParallaxLayer> parallaxLayers;
        Color backgroundColor;

        public static Random RandomObject;

        public Player player;
        Texture2D playerTexture;
        KeyboardState prevKeyboardState;

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
            PixelScaleFactor = 16;
            RandomObject = new Random();

            graphics.PreferredBackBufferHeight = 512;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.ApplyChanges();

            backgroundColor = new Color(86, 138, 205);

            Chunks = new List<TileMap>();

            camera = new Camera2D(GraphicsDevice);
            //cameravelocity = new Vector2(2, 0); //Use -GraphicsDevice.Viewport.Height/128 to get height up to half screenish if paired with chunk-updating y velocity, else, just, peg to player

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

            //Get the count of maps in the maps folder
            string path = Directory.GetCurrentDirectory();
            DirectoryInfo mapDir = new DirectoryInfo(Path.GetFullPath(Path.Combine(path, @".\Content\maps"))); //THIS PATH MAY NEED TO BE AMENDED IN THE FUTURE - Amended 2/20/17 - Why are we using fancy concatenation here anyway?
            mapFiles = mapDir.GetFiles();

            //TODO: Actual map generation script implementation goes here.

            //Starter generation: generate chunks out to the end of the screen to start off.
            //And add the player to the world
            DoStarterGeneration();

            playerTexture = Content.Load<Texture2D>(@"graphics\entity\player");

            SpawnPlayer();

            //Initialize and load background parallaxing layers
            parallaxLayers = new List<ParallaxLayer>();
            //Front Cloud
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_0"), new Vector2(10, .5f), new Vector2(-0.0125f, 0f)));
            //Foreground Leaves
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_2"), new Vector2(30, 4f), new Vector2(-0.5f, 0.125f)));
            //Back Cloud
            parallaxLayers.Add(new ParallaxLayer(Content.Load<Texture2D>(@"graphics\misc\parallax_1"), new Vector2(30, .6f), new Vector2(-0.03f, 0f)));
            //Background Leaves
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //DEBUG/TESTING MOVEMENT. UPDATE WITH CHARACTER/CAMERA UPDATE CALLS
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            KeyboardState keyboardState = Keyboard.GetState();

            player.Update(gameTime, world);

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                player.Move(Movement.Left);
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                player.Move(Movement.Right);
            }
            else
            {
                player.Move(Movement.Stop);
            }

            if (keyboardState.IsKeyDown(Keys.Space) && !prevKeyboardState.IsKeyDown(Keys.Space))
            {
                player.Jump(-15f);
            }

            prevKeyboardState = keyboardState;

            player.Update(gameTime, world);

            //Update camera
            //camera.Position += cameravelocity;

            //Update backgrounds
            foreach (ParallaxLayer p in parallaxLayers)
                p.Update(gameTime, cameravelocity, GraphicsDevice.Viewport); //Replace "direction" with player X velocity

            //Update chunks, camera velocity
            if (UpdateChunkGeneration()) {
                //camera velocity = (same X), delta-y/(distance/x velocity)
                //cameravelocity = new Vector2(cameravelocity.X, (Chunks[0].EndTile.Rect.Y - Chunks[0].StartTile.Rect.Y)/(Chunks[0].Width/cameravelocity.X));

            }

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
                if(p != parallaxLayers[3])
                    p.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix());
            foreach(TileMap map in Chunks)
                map.Draw(spriteBatch); //Draw each active chunk

            player.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            parallaxLayers[3].Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Maybe use this to generate chunks eventually.
        void GenerateNewChunk(Rectangle startTileRect, string mapName) {
            Chunks.Add(new TileMap(startTileRect, Game1.world));
        }

        bool UpdateChunkGeneration() {
            bool needToUpdateTheCamera = false;
            foreach (TileMap chunk in Chunks) {
                if (!chunk.Generated)
                    chunk.Generate(chunk.ReadImage(GetNewMapName()), PixelScaleFactor);
            }


            if (Chunks.Count > 0) {
                if (Chunks[0].EndTile.Rect.X <= camera.Position.X - Chunks[0].EndTile.Rect.Width) {
                    needToUpdateTheCamera = true;
                    //last tile of first chunk is off screen, destroy that chunk (i.e. remove it from the list).
                    Chunks.RemoveAt(0);
                }

                if (Chunks[Chunks.Count - 1].EndTile.Rect.X < GraphicsDevice.Viewport.Width + camera.Position.X) {
                    //End tile of last chunk is on screen, generate new chunk
                    GenerateNewChunk(Chunks[Chunks.Count - 1].EndTile.Rect, GetNewMapName());
                }
            }
            return needToUpdateTheCamera;
        }

        void DoStarterGeneration() {
            Chunks.Add(new TileMap());
        }

        //Gets a random map name from the directory of the maps
        string GetNewMapName() {
            //Return the name of a file from mapFiles (the directory of the maps) 
            return mapFiles[RandomObject.Next(mapFiles.Length)].Name;
        }

        void SpawnPlayer()
        {
            player = new Player(world, playerTexture, new Vector2(PixelScaleFactor, PixelScaleFactor), 10f, UnitConvert.ToWorld(new Vector2(24 * PixelScaleFactor, 20 * PixelScaleFactor)));
        }
    }
}
