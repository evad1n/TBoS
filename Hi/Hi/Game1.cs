using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics;

namespace Hi
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D enemyTexture;
        Texture2D playerTexture;
        Texture2D floorTexture;
        World world;

        Body floor1;
        Body floor2;

        Enemy enemy;
        Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            world = new World(new Vector2(0, 9.8f));

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
            floorTexture = Content.Load<Texture2D>("white");
            enemyTexture = Content.Load<Texture2D>("enemy");
            playerTexture = Content.Load<Texture2D>("player");


            floor1 = BodyFactory.CreateRectangle(world, floorTexture.Width, floorTexture.Height, 1, "ground");
            floor1.BodyType = BodyType.Static;
            floor2 = BodyFactory.CreateRectangle(world, floorTexture.Width, floorTexture.Height, 1, "ground");
            floor2.BodyType = BodyType.Static;

            player = new Player(world, new Vector2(500, 0), playerTexture);
            enemy = new Enemy(world, new Vector2(500, 100), new Vector2(50, 300), new Vector2(650, 300), enemyTexture);

            floor1.Position = new Vector2(0, 300);
            floor2.Position = new Vector2(400, 300);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            world.Step(0.5f);

            player.Update(gameTime);
            enemy.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(floorTexture, floor1.Position, Color.Black);
            spriteBatch.Draw(floorTexture, floor2.Position, Color.Black);
            enemy.Draw(spriteBatch, enemyTexture, floorTexture);
            player.Draw(spriteBatch, playerTexture);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
