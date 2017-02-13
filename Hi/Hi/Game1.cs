using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.DebugView;
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

        KeyboardState oldKeyboardState;

        World world;
        float worldStep;
        Body playerBody;
        Body floor;
        Fixture playerFixture;
        Fixture floorFixture;
        Vertices playerVertices = new Vertices(4);
        Vertices floorVertices = new Vertices(4);

        Vector2 jumpForce;
        float maxJump;
        bool isJumping;
        bool isGrounded;
        float jumpTime;

        Texture2D playerTexture;
        Texture2D floorTexture;

        DebugViewXNA debug;

        //Convert pixels to meters float width = ConvertUnits.ToSimUnits(512f) 
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
            world = new World(new Vector2(0,9.8f));
            jumpForce = new Vector2(0, -10000000000000);
            maxJump = 1f;
            isJumping = false;
            isGrounded = false;
            jumpTime = 0;
            worldStep = 0.1f;

            debug = new DebugViewXNA(world);

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

            playerVertices.Add(new Vector2(0, 0));
            playerVertices.Add(new Vector2(0, playerTexture.Height));
            playerVertices.Add(new Vector2(playerTexture.Width, playerTexture.Height));
            playerVertices.Add(new Vector2(playerTexture.Width, 0));


            floorVertices.Add(new Vector2(0, 0));
            floorVertices.Add(new Vector2(0, floorTexture.Height));   
            floorVertices.Add(new Vector2(floorTexture.Width, floorTexture.Height));
            floorVertices.Add(new Vector2(floorTexture.Width, 0));

            floor = BodyFactory.CreateRectangle(world, floorTexture.Width, floorTexture.Height, 1);
            floor.BodyType = BodyType.Static;

            playerFixture = playerBody.CreateFixture(new PolygonShape(playerVertices, 1));
            floorFixture = floor.CreateFixture(new PolygonShape(floorVertices, 1));

            floor.Position = new Vector2(100, 200);

            playerBody.OnCollision += MyOnCollision;
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

            //Draws debugging view
            var projection = Matrix.CreateOrthographicOffCenter(
            0f,
            ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width),
            ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Height), 0f, 0f,
            1f);
            debug.RenderDebugData(ref projection);

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
                if(isGrounded)
                {
                    isJumping = true;
                    isGrounded = false;
                }
            }
            else
            {
                isJumping = false;
            }

            if(isJumping && state.IsKeyDown(Keys.Space))
            {
                if(jumpTime < maxJump)
                {
                    playerBody.ApplyForce(jumpForce, playerBody.WorldCenter);
                    jumpTime += worldStep/2;
                }
                else
                {
                    isJumping = false;
                }
            }
            else
            {
                jumpTime = 0;
            }

            oldKeyboardState = state;
        }

        public bool MyOnCollision(Fixture b1, Fixture b2, Contact contact)
        {
            isGrounded = true;
            return true;
        }
    }
}
