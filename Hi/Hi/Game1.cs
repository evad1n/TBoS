using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hi
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState oldKeyboardState;

        World world;
        float worldStep;
        Body playerBody;
        Body floor;
        Vector2 jumpForce;
        float maxJump;
        bool isJumping;
        float jumpTime;

        Texture2D playerTexture;
        Texture2D floorTexture;

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
            world = new World(new Vector2(0,500f));
            jumpForce = new Vector2(0, -100000000000);
            maxJump = 0.5f;
            isJumping = false;
            jumpTime = 0;
            worldStep = 0.0167777f;

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
            playerTexture = Content.Load<Texture2D>("tree");
            floorTexture = Content.Load<Texture2D>("white");

            playerBody = BodyFactory.CreateRectangle(world, playerTexture.Width, playerTexture.Height, 1);
            playerBody.BodyType = BodyType.Dynamic;

            floor = BodyFactory.CreateRectangle(world, floorTexture.Width, floorTexture.Height, 1);
            floor.Position = new Vector2(100, 200);
            floor.BodyType = BodyType.Static;
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
            world.Step(worldStep);

            HandleKeyboard();

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
            spriteBatch.Draw(playerTexture, playerBody.Position, Color.White);
            spriteBatch.Draw(floorTexture, floor.Position, Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            if(state.IsKeyDown(Keys.A))
            {
                playerBody.Position = new Vector2(playerBody.Position.X -5, playerBody.Position.Y);
            }
            if (state.IsKeyDown(Keys.D))
            {
                playerBody.Position = new Vector2(playerBody.Position.X + 5, playerBody.Position.Y);
            }

            if(state.IsKeyDown(Keys.Space))
            {
                isJumping = true;
            }
            else
            {
                isJumping = false;
                jumpTime = 0;
            }

            if(isJumping && jumpTime < maxJump)
            {
                playerBody.ApplyForce(jumpForce, playerBody.WorldCenter);
                jumpTime += worldStep;
            }

            oldKeyboardState = state;
        }
    }
}
