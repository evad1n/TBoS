using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PlayerAnimationTesting {
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game {

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		int frame = 0;
		int drawFrame = 0;
		int scaleSize = 4;


		Texture2D playerSpriteSheet;						//XXXXXXXXXXXXXXXXXX
		Dictionary<string, Rectangle[]> animations;			//XXXXXXXXXXXXXXXXXX

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
			// TODO: Add your initialization logic here

			animations = new Dictionary<string, Rectangle[]>(); //XXXXXXXXXXXXXXXXXXXXXXXX

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			playerSpriteSheet = Content.Load<Texture2D>("Player Sprite");
			animations["stand"] = new Rectangle[] { new Rectangle(0, 0, 8, 8), new Rectangle(8, 0, 8, 8) };
			animations["walk"] = new Rectangle[] { new Rectangle(8, 0, 8, 8), new Rectangle(8, 8, 8, 8), new Rectangle(8, 16, 8, 8)};
			animations["jump"] = new Rectangle[] { new Rectangle(16, 0, 8, 8), new Rectangle(16, 8, 8, 8), new Rectangle(16, 16, 8, 8) };
			animations["roll"] = new Rectangle[] { new Rectangle(16, 0, 8, 8)};
			animations["midair"] = new Rectangle[] { new Rectangle(8, 24, 8, 8), new Rectangle(16, 24, 8, 8), new Rectangle(24, 24, 8, 8), new Rectangle(16, 8, 8, 8), new Rectangle(24, 16, 8, 8) };

			// TODO: use this.Content to load your game content here
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

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.CornflowerBlue);

			

			spriteBatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp);

			spriteBatch.Draw(playerSpriteSheet, new Rectangle(50, 50, 8 * scaleSize, 8 * scaleSize), animations["stand"][drawFrame], Color.White);

			spriteBatch.Draw(playerSpriteSheet, new Rectangle(100, 50, 8 * scaleSize, 8 * scaleSize), animations["stand"][drawFrame], Color.White);

			spriteBatch.Draw(playerSpriteSheet, new Rectangle(50, 50, 8 * scaleSize, 8 * scaleSize), animations["stand"][drawFrame], Color.White);

			spriteBatch.Draw(playerSpriteSheet, new Rectangle(50, 50, 8 * scaleSize, 8 * scaleSize), animations["stand"][drawFrame], Color.White);

			spriteBatch.Draw(playerSpriteSheet, new Rectangle(50, 50, 8 * scaleSize, 8 * scaleSize), animations["stand"][drawFrame], Color.White);

			spriteBatch.End();

			frame++;
			if (frame - 30 == 0)
				drawFrame = 1;
			if (frame - 60 == 0) {
				drawFrame = 0;
				frame = 0;
			}
			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}
